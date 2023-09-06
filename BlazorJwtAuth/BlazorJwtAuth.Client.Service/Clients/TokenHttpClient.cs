using System.Net.Http.Json;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Clients;

public class TokenHttpClient
{
    private readonly HttpClient _http;

    public TokenHttpClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<WeatherForecastDto[]?> GetForecastAsync(AppSettings appSettings)
    {
        try
        {
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

    public async Task<AuthenticationResponse> GetTokenAsync(AppSettings appSettings, GetTokenRequest getTokenRequest)
    {
        try
        {
            var response = await _http.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/token", getTokenRequest);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
            if (result is null)
                return new AuthenticationResponse
                {
                    Detail = "Unable to deserialize response from server.",
                    IsAuthenticated = false,
                    Message = "Sorry, we were unable to authenticate you at this time. Please try again shortly."
                };

            return new AuthenticationResponse
            {
                Detail = result.Detail,
                IsAuthenticated = result.IsAuthenticated,
                Message = result.Message,
                Status = result.Status,
                Token = result.Token,
                UserName = result.UserName,
                Email = result.Email,
                RefreshToken = result.RefreshToken,
                RefreshTokenExpiration = result.RefreshTokenExpiration,
                Roles = result.Roles
            };
        }
        catch (Exception ex)
        {
            return new AuthenticationResponse
            {
                Detail = ex.Message,
                IsAuthenticated = false,
                Message = "Sorry, some problem occurred while authenticating you. Please try again."
            };
        }
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(AppSettings appSettings,
        RefreshTokenRequest refreshTokenRequest)
    {
        try
        {
            var response = await _http.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/refresh-token",
                refreshTokenRequest);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
            if (result is null)
                return new AuthenticationResponse
                {
                    Detail = "Unable to deserialize response from server.",
                    IsAuthenticated = false,
                    Message = "Sorry, we were unable to refresh a token. Please try again shortly."
                };

            return new AuthenticationResponse
            {
                Detail = result.Detail,
                IsAuthenticated = result.IsAuthenticated,
                Message = result.Message,
                Status = result.Status,
                Token = result.Token,
                UserName = result.UserName,
                Email = result.Email,
                RefreshToken = result.RefreshToken,
                RefreshTokenExpiration = result.RefreshTokenExpiration,
                Roles = result.Roles
            };
        }
        catch (Exception ex)
        {
            return new AuthenticationResponse
            {
                Detail = ex.Message,
                IsAuthenticated = false,
                Message = "Sorry, some problem occurred while authenticating you. Please try again."
            };
        }
    }
}
