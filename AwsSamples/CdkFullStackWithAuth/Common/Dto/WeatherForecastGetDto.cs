using Common.Models;

namespace Common.Dto;

public class WeatherForecastGetDto : ResponseBaseDto
{
    public List<WeatherForecastModel> Data { get; set; } = default!;
}
