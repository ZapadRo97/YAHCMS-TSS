using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using YAHCMS.UserService.Persistence;

namespace YAHCMS.UserService
{
    public class Startup
    {
        private ILogger logger;
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment {get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //logging stuff
            var serviceProvider = services.BuildServiceProvider();
            logger = serviceProvider.GetService<ILogger<Startup>>();
            services.AddSingleton(typeof(ILogger), logger);

            //database conn stuff
            var transient = false;
            if(Configuration["transient"] != null) {
                transient = Boolean.Parse(Configuration["transient"]);
            }

            logger.LogInformation("Using SQL Server repository");
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddEntityFrameworkSqlServer()
                .AddDbContext<UserDbContext>(options 
                    => options.UseSqlServer(Configuration["connectionString"]));


            //identity stuff

            
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            IdentityModelEventSource.ShowPII = true;

            string domain = Configuration["Auth0Domain"];
            services.AddAuthentication(options =>
            {
                //AuthenticationOptions s;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = "yahcms";
                //options.RequireHttpsMetadata = false;
            });

            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:users", policy => policy.Requirements.Add(new HasScopeRequirement("read:users", domain)));
                options.AddPolicy("write:users", policy => policy.Requirements.Add(new HasScopeRequirement("write:users", domain)));
            });
            
            

    

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
