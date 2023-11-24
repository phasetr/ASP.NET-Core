using Common.Constants;
using Common.Dto;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ApiPath.WeatherForecastRoot)]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public WeatherForecastGetDto Get()
    {
        return new WeatherForecastGetDto
        {
            Message = "Weather forecast for 5 days",
            Succeeded = true,
            Data = new List<WeatherForecastModel>
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
            }
        };
    }
}
