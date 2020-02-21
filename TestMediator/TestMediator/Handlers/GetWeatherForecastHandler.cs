using System.Threading;
using System.Threading.Tasks;
using MediatR;
using testMediator.Models;

namespace testMediator.Handlers
{
    public class GetWeatherForecastHandler  : IRequestHandler<GetWeatherForecast, GetWeatherForecast>
    {
        public async Task<GetWeatherForecast> Handle(GetWeatherForecast request, CancellationToken cancellationToken)
        {
            request.AddTemperature();
            return await Task.FromResult(request);
        }
    }
}