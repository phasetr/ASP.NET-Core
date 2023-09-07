using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface IWeatherForecastHttpClientService
{
    Task<WeatherForecastDto[]?> GetForecastAsync(AppSettings appSettings);
}
