using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.API.Helper;
using PortolWeb.Entities;
using Serilog.Events;
using Serilog.Core;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;

namespace PortolWeb.API
{
    public class Program
    {
        //public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        //   .SetBasePath(Directory.GetCurrentDirectory())
        //   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //   .AddEnvironmentVariables()
        //   .Build();

        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

          
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {

                    IDatabaseManagement dbManagement = services.GetRequiredService<IDatabaseManagement>();

                 //   var hosting = services.GetRequiredService<IHostingEnvironment>(); 

                    var appSettings = services.GetRequiredService<IOptions<AppSettings>>();

                    Log.Logger = new LoggerConfiguration()
                               .MinimumLevel.Debug()
                               .WriteTo.MSSqlServer(appSettings.Value.ConnectionString ,"tblLogErrors", autoCreateSqlTable:true)
                               .WriteTo.File(appSettings.Value.LogPaht, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", rollingInterval:RollingInterval.Day, restrictedToMinimumLevel:LogEventLevel.Warning)
                               .Enrich.WithProperty("PortolWeb.Api", "A!!")
                               .CreateLogger();


                    dbManagement.UpgradeDB(appSettings.Value.DBScriptPath);
                }
                catch (Exception ex)
                {
                    //var logger = services.GetRequiredService<ILogger<Program>>();
                    //logger.LogError(ex, "An error occurred.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseSerilog();
    }
}
