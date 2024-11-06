using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Roaster.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Counter<int> _hits;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            _hits = meterFactory.Create("Roaster.Api.Roasts").CreateCounter<int>("Roaster.GetWeatherForecast.Hits");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _hits.Add(1);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
