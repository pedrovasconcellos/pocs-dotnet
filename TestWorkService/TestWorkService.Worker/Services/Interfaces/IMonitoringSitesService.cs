using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWorkService.Worker.Models;

namespace TestWorkService.Worker.Services.Interfaces
{
    /// <summary>
    /// Iinterface monitoring sites service
    /// </summary>
    public interface IMonitoringSitesService
    {
        /// <summary>
        /// Method
        /// </summary>
        /// <returns></returns>
        Task<IList<MonitoringResult>> Work();
    }
}
