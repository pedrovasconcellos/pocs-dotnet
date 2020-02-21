using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestWorkService.Worker.Services.Interfaces;

namespace TestWorkService.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        internal readonly IConfiguration _configuration;
        private readonly IMonitoringSitesService _service;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IMonitoringSitesService service)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this._logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await this._service.Work();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
