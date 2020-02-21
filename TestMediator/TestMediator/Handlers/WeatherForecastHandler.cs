using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using testMediator.Models;

namespace testMediator.Handlers
{
    public class WeatherForecastHandler  : IRequestHandler<WeatherForecast, WeatherForecast>
    {
        public async Task<WeatherForecast> Handle(WeatherForecast request, CancellationToken cancellationToken)
        {
            if (request.Date == null)
                throw new Exception("Invalid date.");

            request.AddTemperature();
            return await Task.FromResult(request);
        }
    }
}