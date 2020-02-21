using System;
using MediatR;

namespace testMediator.Models
{
    public class GetWeatherForecast  : IRequest<GetWeatherForecast>
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly Random _random = new Random();

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public void AddTemperature() 
        {
            this.TemperatureC = _random.Next(-20, 55);
            this.Summary = _summaries[_random.Next(_summaries.Length)];
        }
    }
}
