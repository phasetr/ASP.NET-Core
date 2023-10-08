using Common.Dto;

namespace Client.Services.Interfaces;

public interface IWeatherForecastHttpClientService
{
    Task<WeatherForecastResponseDto[]?> GetForecastAsync(HttpClient httpClient);
}
