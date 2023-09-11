using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Services;

public class TokenHttpClientService : ITokenHttpClientService
{
    public async Task<AuthenticationResponse> GetTokenAsync(AppSettings appSettings, HttpClient httpClient,
        GetTokenRequest getTokenRequest)
    {
        try
        {
            var response =
                await httpClient.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/token", getTokenRequest);
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
                Email = result.Email,
                IsAuthenticated = result.IsAuthenticated,
                Message = result.Message,
                RefreshToken = result.RefreshToken,
                RefreshTokenExpiration = result.RefreshTokenExpiration,
                Roles = result.Roles,
                Status = result.Status,
                Token = result.Token,
                UserName = result.UserName
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
        HttpClient httpClient,
        RefreshTokenRequest refreshTokenRequest)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/refresh-token",
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
