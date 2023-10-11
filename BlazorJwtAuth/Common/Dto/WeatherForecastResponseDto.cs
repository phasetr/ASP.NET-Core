using Common.Models;

namespace Common.Dto;

public class WeatherForecastResponseDto
{
    public WeatherForecast[] WeatherForecasts { get; set; } = default!;
}
