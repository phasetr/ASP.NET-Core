using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Services.Interfaces;

namespace BlazorJwtAuth.Client.Service.Services;

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

    public async Task<WeatherForecastResponseDto[]?> GetForecastAsync(HttpClient httpClient)
    {
        try
        {
            var token = await _tokenService.GetTokenAsync();
            if (token.Expiration > _ptDateTime.UtcNow)
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Token);
            var response = await httpClient
                .GetAsync(ApiPath.V1WeatherForecastFull);
            var result = await response.Content.ReadFromJsonAsync<WeatherForecastResponseDto[]>();
            return result ?? Array.Empty<WeatherForecastResponseDto>();
        }
        catch
        {
            return Array.Empty<WeatherForecastResponseDto>();
        }
    }
}
