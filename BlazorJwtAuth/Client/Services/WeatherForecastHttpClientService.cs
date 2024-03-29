using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.Services.Interfaces;
using Common.Constants;
using Common.Dto;
using Common.Models;
using Common.Services.Interfaces;

namespace Client.Services;

public class WeatherForecastHttpClientService : IWeatherForecastHttpClientService
{
    private readonly IPtDateTime _ptDateTime;
    private readonly ITokenService _tokenService;

    public WeatherForecastHttpClientService(
        IPtDateTime ptDateTime,
        ITokenService tokenService)
    {
        _ptDateTime = ptDateTime;
        _tokenService = tokenService;
    }

    public async Task<WeatherForecast[]?> GetForecastAsync(HttpClient httpClient)
    {
        try
        {
            var token = await _tokenService.GetTokenAsync();
            if (token.Expiration > _ptDateTime.UtcNow)
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Token);
            var response = await httpClient
                .GetAsync(ApiPath.V1WeatherForecastFull);
            var result = await response.Content.ReadFromJsonAsync<WeatherForecastResponseDto>();
            return result?.WeatherForecasts ?? Array.Empty<WeatherForecast>();
        }
        catch
        {
            return Array.Empty<WeatherForecast>();
        }
    }
}
