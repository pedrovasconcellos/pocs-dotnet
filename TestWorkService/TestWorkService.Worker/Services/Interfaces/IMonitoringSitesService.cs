using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWorkService.Worker.Models;

namespace TestWorkService.Worker.Services.Interfaces
{
    public interface IMonitoringSitesService
    {
        Task<IList<MonitoringResult>> Work();
    }
}
