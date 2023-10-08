using Common.Dto;

namespace Client.Service.Services.Interfaces;

public interface IWeatherForecastHttpClientService
{
    Task<WeatherForecastResponseDto[]?> GetForecastAsync(HttpClient httpClient);
}
