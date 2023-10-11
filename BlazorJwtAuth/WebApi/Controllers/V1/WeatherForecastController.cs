using System;
using System.Linq;
using Common.Constants;
using Common.Dto;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1;

[Authorize]
[ApiController]
[Route(ApiPath.V1WeatherForecastFull)]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Random _random;

    public WeatherForecastController(Random random)
    {
        _random = random;
    }

    [HttpGet]
    public WeatherForecastResponseDto Get()
    {
        return new WeatherForecastResponseDto
        {
            WeatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = _random.Next(-20, 55),
                    Summary = Summaries[_random.Next(Summaries.Length)]
                })
                .ToArray()
        };
    }
}
