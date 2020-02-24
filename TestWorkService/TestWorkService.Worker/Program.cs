using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TestWorkService.WebAPI;
using TestWorkService.Worker.Services;
using TestWorkService.Worker.Services.Interfaces;

namespace TestWorkService.Worker
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Task.Run(() => CreateHostBuilder(args).Build().Run());
            CreateWebHostBuilder(args).Build().Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IMonitoringSitesService, MonitoringSitesService>();
                    services.AddHostedService<Worker>();
                });
        }

        internal static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((webBuilderContext, configBuilder) =>
                    {
                        ConfigureAppConfigurationJson(webBuilderContext, configBuilder);
                    })
                    .UseKestrel()
                    .ConfigureKestrel((webBuilderContext, serverOptions) =>
                    {
                        ConfigureKestrelHttp(webBuilderContext, serverOptions);
                        ConfigureKestrelHttps(webBuilderContext, serverOptions);
                    })
                    .UseStartup<Startup>();
                });
        }

        private static IConfigurationRoot ConfigureAppConfigurationJson(WebHostBuilderContext webBuilderContext, IConfigurationBuilder configBuilder)
        {
            var hostEnvironment = webBuilderContext.HostingEnvironment;
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true);

            configBuilder.AddEnvironmentVariables();
            return configBuilder.Build();
        }

        private static bool ConfigureKestrelHttp(WebHostBuilderContext webBuilderContext, KestrelServerOptions serverOptions)
        {

            var port = webBuilderContext.Configuration.GetValue<int>("Port:HttpPort");
            if (port <= 0)
                return false;

            serverOptions.Listen(IPAddress.Any, port);
            serverOptions.Listen(IPAddress.Loopback, port);

            return true;
        }

        private static bool ConfigureKestrelHttps(WebHostBuilderContext webBuilderContext, KestrelServerOptions serverOptions)
        {
            var configuration = webBuilderContext.Configuration;
            var subjectName = configuration.GetValue<string>("DefaultCertificate:SubjectName");
            var certificate = GetCertificate(subjectName);

            var port = configuration.GetValue<int>("Port:HttpsPort");
            var sslConfigFileName = configuration.GetValue<string>("Certificate:FileName");
            var sslConfigPassword = configuration.GetValue<string>("Certificate:Password");
            var sslConfigIsValid = !(string.IsNullOrEmpty(sslConfigFileName) && string.IsNullOrEmpty(sslConfigPassword));

            if ((certificate == null && !sslConfigIsValid) || port <= 0)
                return false;

            serverOptions.Listen(IPAddress.Any, port, listenOptions =>
            {
                if (sslConfigIsValid)
                    listenOptions.UseHttps(sslConfigFileName, sslConfigPassword);

                if (certificate != null && !sslConfigIsValid)
                    listenOptions.UseHttps(certificate);
            });
            serverOptions.Listen(IPAddress.Loopback, port, listenOptions =>
            {
                if (sslConfigIsValid)
                    listenOptions.UseHttps(sslConfigFileName, sslConfigPassword);

                if (certificate != null && !sslConfigIsValid)
                    listenOptions.UseHttps(certificate);
            });

            return true;
        }

        private static X509Certificate2 GetCertificate(string subjectName)
        {
            using (var store = new X509Store(StoreName.My))
            {
                store.Open(OpenFlags.ReadOnly);      
                var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, false);
                if (certificates.Count > 0)
                    return certificates[0];
            }
            return null;
        }
    }
}
