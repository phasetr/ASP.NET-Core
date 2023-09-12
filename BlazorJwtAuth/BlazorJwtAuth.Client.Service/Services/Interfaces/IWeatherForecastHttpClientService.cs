using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface IWeatherForecastHttpClientService
{
    Task<WeatherForecastResponseDto[]?> GetForecastAsync(HttpClient httpClient);
}
