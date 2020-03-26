using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using YAHCMS.WebApp.Services;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace YAHCMS.WebApp
{
    public class Startup
    {

        private ILogger logger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<ApiEndpoints>(Configuration);

            var serviceProvider = services.BuildServiceProvider();
            logger = serviceProvider.GetService<ILogger<Startup>>();
            services.AddSingleton(typeof(ILogger), logger);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
              .AddCookie()
              .AddOpenIdConnect("Auth0", options =>
              {
                  // Set the authority to your Auth0 domain
                  options.Authority = $"https://{Configuration["Auth0:Domain"]}";

                  // Configure the Auth0 Client ID and Client Secret
                  options.ClientId = Configuration["Auth0:ClientId"];
                  options.ClientSecret = Configuration["Auth0:ClientSecret"];

                  options.SaveTokens = true;
                  // Set response type to code
                  options.ResponseType = "code";

                  options.RequireHttpsMetadata = false;

                  // Configure the scope
                  options.Scope.Clear();
                  options.Scope.Add("openid");

                  // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                  // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                  options.CallbackPath = new PathString("/signin-auth0");

                  // Configure the Claims Issuer to be Auth0
                  options.ClaimsIssuer = "Auth0";


                  options.Events = new OpenIdConnectEvents
                  {
                      OnRedirectToIdentityProvider = context =>
                      {
                          context.ProtocolMessage.Parameters.Add("audience", "yahcms");

                          return Task.CompletedTask;
                      },

                      OnRedirectToIdentityProviderForSignOut = (context) =>
                      {
                          var logoutUri = $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";
                          var postLogoutUri = context.Properties.RedirectUri;
                          if (!string.IsNullOrEmpty(postLogoutUri))
                          {
                              if (postLogoutUri.StartsWith("/"))
                              {
                                  // transform to absolute
                                  var request = context.Request;
                                  postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                              }

                              logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                          }

                          context.Response.Redirect(logoutUri);
                          context.HandleResponse();

                          

                          return Task.CompletedTask;
                      },

                      OnTicketReceived = context =>
                      {
                          // Get the ClaimsIdentity
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                                // Add the Name ClaimType. This is required if we want User.Identity.Name to actually return something!
                                if (!context.Principal.HasClaim(c => c.Type == ClaimTypes.Name) &&
                                identity.HasClaim(c => c.Type == "name"))
                                identity.AddClaim(new Claim(ClaimTypes.Name, identity.FindFirst("name").Value));

                                // Check if token names are stored in Properties
                                if (context.Properties.Items.ContainsKey(".TokenNames"))
                                {   
                                    // Token names a semicolon separated
                                    string[] tokenNames = context.Properties.Items[".TokenNames"].Split(';');

                                    // Add each token value as Claim
                                    foreach (var tokenName in tokenNames)
                                    {
                                        // Tokens are stored in a Dictionary with the Key ".Token.<token name>"
                                        string tokenValue = context.Properties.Items[$".Token.{tokenName}"];
                                        identity.AddClaim(new Claim(tokenName, tokenValue));
                                    }
                                }
                        }

                        return Task.CompletedTask;
                    }

                };


            });


            services.AddHttpClient<AuthManagementClient>();
            services.AddHttpClient<BlogAndPostClient>();
            services.AddHttpClient<CulturalClient>();
            services.AddHttpClient<UserClient>();

            //comment these before deploy
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:AccessKey"]);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS:SecretKey"]);
            Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS:Region"]);

            services.AddSingleton<IS3Service, S3Service>();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
           
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                    //.RequireAuthorization();
            });
            
        }
    }
}