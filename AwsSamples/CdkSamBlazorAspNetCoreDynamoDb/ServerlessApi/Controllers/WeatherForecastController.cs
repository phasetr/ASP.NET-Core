using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace ServerlessApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public List<WeatherForecast> Get()
    {
        return new List<WeatherForecast>
        {
            new()
            {
                Date = new DateTime(2018, 5, 6),
                TemperatureC = 1,
                Summary = "Freezing"
            },
            new()
            {
                Date = new DateTime(2018, 5, 7),
                TemperatureC = 14,
                Summary = "Bracing"
            },
            new()
            {
                Date = new DateTime(2018, 5, 8),
                TemperatureC = -13,
                Summary = "Freezing"
            },
            new()
            {
                Date = new DateTime(2018, 5, 9),
                TemperatureC = -16,
                Summary = "Balmy"
            },
            new()
            {
                Date = new DateTime(2018, 5, 10),
                TemperatureC = -2,
                Summary = "Chilly"
            }
        };
    }
}
