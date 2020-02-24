
namespace TestWorkService.Worker.Models
{
    /// <summary>
    /// Monitoring result
    /// </summary>
    public class MonitoringResult
    {
        /// <summary>
        /// Horary
        /// </summary>
        public string Horary { get; set; }
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Exception
        /// </summary>
        public object Exception { get; set; }
    }
}
