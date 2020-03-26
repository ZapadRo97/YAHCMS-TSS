using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YAHCMS.CulturalService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var config = new ConfigurationBuilder()
                            .AddCommandLine(args)
                            .Build();

                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == "Development";
                    if(!isDevelopment)
                        webBuilder.UseUrls("http://0.0.0.0:5007");
                    webBuilder.UseConfiguration(config);
                    webBuilder.UseStartup<Startup>();
                    
                });
    }
}
