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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            const int LedPinNumber = 24;
            const int ButtonPinNumber = 4;

            GpioController controller = new GpioController(PinNumberingScheme.Logical);

            controller.OpenPin(ButtonPinNumber, PinMode.InputPullUp);
            controller.OpenPin(LedPinNumber, PinMode.Output);

            var blinkCount = 0;
            bool heartBeat = false;
            while (controller.Read(ButtonPinNumber) == PinValue.High
                && blinkCount++ < 30)
            {
                heartBeat = !heartBeat;
                controller.Write(LedPinNumber, heartBeat ? PinValue.High : PinValue.Low);
                Thread.Sleep(500);
            }


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
