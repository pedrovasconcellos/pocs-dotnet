using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TestWorkService.WebAPI.Controllers
{
    /// <summary>
    /// DocumentObjectController
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class DocumentObjectController : ControllerBase
    {
        private readonly ILogger<DocumentObjectController> _logger;
        private static readonly IList<string> _names = new string [] { 
            "Pedro",
            "Charles",
            "Shon",
            "Anderson"
        };

        /// <summary>
        /// DocumentObjectController builder
        /// </summary>
        /// <param name="logger"></param>
        public DocumentObjectController(ILogger<DocumentObjectController> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<string>>> Get(string name)
        {
            this._logger.LogDebug($"{this.GetType().Name}.{nameof(this.Get)}", new { name });

            var result = default(List<string>);
            if (!string.IsNullOrEmpty(name))
                result = _names.Where(x => x.Contains(name)).ToList();
            else
                result = _names.ToList();

            return await Task.FromResult(this.Ok(result));
        }
    }
}
