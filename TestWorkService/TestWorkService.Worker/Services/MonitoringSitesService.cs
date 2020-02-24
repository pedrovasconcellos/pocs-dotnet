
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TestWorkService.Worker.Models;
using TestWorkService.Worker.Services.Interfaces;

namespace TestWorkService.Worker.Services
{
    /// <summary>
    /// Monitoring sites service
    /// </summary>
    public class MonitoringSitesService : IMonitoringSitesService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServiceConfigurations _serviceConfigurations;

        /// <summary>
        /// Monitoring sites service builder
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public MonitoringSitesService(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;

            this._serviceConfigurations = new ServiceConfigurations();
            
            new ConfigureFromConfigurationOptions<ServiceConfigurations>(
                configuration.GetSection("ServiceConfigurations"))
                    .Configure(this._serviceConfigurations);
        }

        /// <summary>
        /// Method work
        /// </summary>
        /// <returns></returns>
        public async Task<IList<MonitoringResult>> Work()
        {
            IList<MonitoringResult> results = new List<MonitoringResult>();

            foreach (string host in this._serviceConfigurations.Hosts)
            {
                _logger.LogInformation($"Checking host [{host}] availability");

                var result = new MonitoringResult
                {
                    Horary = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Host = host,
                };

                try
                {
                    using Ping ping = new Ping();
                    var response = ping.Send(host);
                    result.Status = response.Status.ToString();
                }
                catch (Exception ex)
                {
                    result.Status = "Exception";
                    result.Exception = ex;
                }

                string jsonResult = JsonConvert.SerializeObject(result);
                
                if (result.Exception == null)
                    _logger.LogInformation($"{Environment.NewLine}{jsonResult}{Environment.NewLine}");
                else
                    _logger.LogError($"{Environment.NewLine}{jsonResult}{Environment.NewLine}");

                results.Add(result);
            }

            return await Task.FromResult(results);
        }
    }
}
