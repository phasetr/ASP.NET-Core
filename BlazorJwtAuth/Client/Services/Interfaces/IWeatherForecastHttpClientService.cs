using Common.Models;

namespace Client.Services.Interfaces;

public interface IWeatherForecastHttpClientService
{
    Task<WeatherForecast[]?> GetForecastAsync(HttpClient httpClient);
}
