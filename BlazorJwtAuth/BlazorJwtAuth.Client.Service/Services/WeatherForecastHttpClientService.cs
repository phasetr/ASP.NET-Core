using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Services.Interfaces;

namespace BlazorJwtAuth.Client.Service.Services;

public class WeatherForecastHttpClientService : IWeatherForecastHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPtDateTime _ptDateTime;
    private readonly ITokenService _tokenService;

    public WeatherForecastHttpClientService(
        IHttpClientFactory httpClientFactory,
        IPtDateTime ptDateTime,
        ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _ptDateTime = ptDateTime;
        _tokenService = tokenService;
    }

    public async Task<WeatherForecastDto[]?> GetForecastAsync(AppSettings appSettings)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = await _tokenService.GetTokenAsync();
            if (token.Expiration > _ptDateTime.UtcNow)
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", $"{token.Token}");
            var response =
                await httpClient.GetFromJsonAsync<WeatherForecastDto[]>(
                    $"{appSettings.ApiBaseAddress}/WeatherForecast");
            return response ?? Array.Empty<WeatherForecastDto>();
        }
        catch
        {
            return Array.Empty<WeatherForecastDto>();
        }
    }
}
