using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using TestWorkService.WebAPI;
using TestWorkService.Worker.Services;
using TestWorkService.Worker.Services.Interfaces;

namespace TestWorkService.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(() => CreateHostBuilder(args).Build().Run());
            
            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    ConfigureApplicationSettings(hostingContext, config);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IMonitoringSitesService, MonitoringSitesService>();
                    services.AddHostedService<Worker>();
                });
        }

        private static void ConfigureApplicationSettings(WebHostBuilderContext hostingContext, IConfigurationBuilder configuration)
        {
            var env = hostingContext.HostingEnvironment;
            configuration.SetBasePath(Directory.GetCurrentDirectory());
            configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);

            configuration.AddEnvironmentVariables();
            configuration.Build();
        }
    }
}
