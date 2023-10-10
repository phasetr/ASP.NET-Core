using System;
using System.Collections.Generic;
using System.Linq;
using Common.Constants;
using Common.Dto;
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
    public IEnumerable<WeatherForecastResponseDto> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecastResponseDto
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = _random.Next(-20, 55),
                Summary = Summaries[_random.Next(Summaries.Length)]
            })
            .AsEnumerable();
    }
}
