using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChickNetWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            //"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            "Varmt", "Køligt", "Lummert",
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILifeTimeChecker _lifeTimeChecker;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ILifeTimeChecker lifeTimeChecker)
        {
            _logger = logger;
            _lifeTimeChecker = lifeTimeChecker;
            _lifeTimeChecker.UseCount++;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return
                Enumerable
                    .Range(1, 5)
                    .Select(index =>
                        new WeatherForecast
                        {
                            Date = DateTime.Now.AddDays(index),
                            TemperatureC = _lifeTimeChecker.UseCount, // rng.Next(-20, 55),
                            Summary = Summaries[rng.Next(Summaries.Length)]
                        })
                    .ToArray();
        }
    }

    public interface ILifeTimeChecker
    {
        int UseCount { get; set; }
    }

    public class LifeTimeChecker : ILifeTimeChecker
    {
        public int UseCount { get; set; }
    }
}
