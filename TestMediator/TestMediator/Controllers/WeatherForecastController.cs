using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using testMediator.Models;

namespace testMediator.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IMediator _mediator { get; set; }
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            this._logger = logger;
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            var result = Enumerable.Range(1, 5)
            .Select(index => this._mediator.Send(new GetWeatherForecast()).Result)
            .ToArray();

            return await Task.FromResult(this.Ok(result));
        }
        
        [HttpPost]
        public async Task<ActionResult<WeatherForecast>> Post([FromBody]WeatherForecast request)
        {
            var response = await this._mediator.Send(request);
            return this.Ok(response);          
        }
    }
}
