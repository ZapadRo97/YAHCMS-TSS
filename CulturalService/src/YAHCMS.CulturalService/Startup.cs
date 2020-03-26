using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YAHCMS.CulturalService.Persistence;

namespace YAHCMS.CulturalService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //database conn stuff
            //logger.LogInformation("Using SQL Server repository");
                services.AddScoped<IArtistRepository, ArtistRepository>();
                services.AddScoped<ILocationRepository, LocationRepository>();
                services.AddScoped<IQuizRepository, QuizRepository>();
                services.AddEntityFrameworkSqlServer()
                .AddDbContext<CulturalDbContext>(options 
                    => options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                //AuthenticationOptions s;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:artists", policy => policy.Requirements.Add(new HasScopeRequirement("read:artists", domain)));
                options.AddPolicy("write:artists", policy => policy.Requirements.Add(new HasScopeRequirement("write:artists", domain)));
            
                options.AddPolicy("read:quiz", policy => policy.Requirements.Add(new HasScopeRequirement("read:quiz", domain)));
                options.AddPolicy("write:quiz", policy => policy.Requirements.Add(new HasScopeRequirement("write:quiz", domain)));

                options.AddPolicy("read:location", policy => policy.Requirements.Add(new HasScopeRequirement("read:location", domain)));
                options.AddPolicy("write:location", policy => policy.Requirements.Add(new HasScopeRequirement("write:location", domain)));
            });

            // ********************
            // Setup CORS
            // ********************
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            //corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.WithOrigins("http://localhost:5100", "http://52.89.227.233:5000");
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // ********************
            // USE CORS - might not be required.
            // ********************
            app.UseCors("SiteCorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
