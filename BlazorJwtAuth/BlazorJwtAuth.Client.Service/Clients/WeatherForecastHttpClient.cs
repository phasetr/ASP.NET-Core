using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Clients;

public class WeatherForecastHttpClient
{
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;

    public WeatherForecastHttpClient(
        HttpClient http,
        ITokenService tokenService)
    {
        _http = http;
        _tokenService = tokenService;
    }

    public async Task<WeatherForecastDto[]?> GetForecastAsync(AppSettings appSettings)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token.Expiration > DateTime.UtcNow)
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", $"{token.Token}");

            var response =
                await _http.GetFromJsonAsync<WeatherForecastDto[]>(
                    $"{appSettings.ApiBaseAddress}/WeatherForecast");
            return response ?? Array.Empty<WeatherForecastDto>();
        }
        catch
        {
            return Array.Empty<WeatherForecastDto>();
        }
    }
}
