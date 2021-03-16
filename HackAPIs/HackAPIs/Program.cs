using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HackAPIs.Services.Db;
using HackAPIs.Services.Db.Data;
using System;

namespace HackAPIs 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //         CreateHostBuilder(args).Build().Run();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<NurseHackContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();

            // Process Result.      
        }

        /*
        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();

                    config.AddAzureAppConfiguration(options =>
                    {
                        options.Connect(settings["ConnectionStrings:AppConfig"])
                                .ConfigureKeyVault(kv =>
                                {
                                    kv.SetCredential(new DefaultAzureCredential());
                                });
                    });
                })
                .UseStartup<Startup>());
    }
        */

        public static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<Startup>();
                  });
    }

}
