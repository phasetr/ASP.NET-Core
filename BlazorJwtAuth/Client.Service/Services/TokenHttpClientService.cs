using System.Net.Http.Json;
using Client.Service.Services.Interfaces;
using Common.Constants;
using Common.Dto;

namespace Client.Service.Services;

public class TokenHttpClientService : ITokenHttpClientService
{
    public async Task<AuthenticationResponseDto> RefreshTokenAsync(HttpClient httpClient,
        RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(ApiPath.V1UserRefreshTokenFull, refreshTokenDto);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
            if (result is null)
                return new AuthenticationResponseDto
                {
                    IsAuthenticated = false,
                    Message = "Sorry, we were unable to refresh a token. Please try again shortly."
                };

            return new AuthenticationResponseDto
            {
                IsAuthenticated = result.IsAuthenticated,
                Message = result.Message,
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
            return new AuthenticationResponseDto
            {
                Errors = new List<string> {ex.Message},
                IsAuthenticated = false,
                Message = "Sorry, some problem occurred while authenticating you. Please try again.",
                Succeeded = false
            };
        }
    }

    public async Task<AuthenticationResponseDto> GetTokenAsync(HttpClient httpClient,
        GetTokenResponseDto getTokenResponseDto)
    {
        try
        {
            var response =
                await httpClient.PostAsJsonAsync(ApiPath.V1UserGetTokenFull, getTokenResponseDto);
            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
            if (result is null)
                return new AuthenticationResponseDto
                {
                    Errors = new List<string> {"A server response is null."},
                    IsAuthenticated = false,
                    Message = "Sorry, we were unable to authenticate you at this time. Please try again shortly.",
                    Succeeded = false
                };

            return new AuthenticationResponseDto
            {
                Email = result.Email,
                IsAuthenticated = result.IsAuthenticated,
                Message = result.Message,
                RefreshToken = result.RefreshToken,
                RefreshTokenExpiration = result.RefreshTokenExpiration,
                Roles = result.Roles,
                Token = result.Token,
                UserName = result.UserName
            };
        }
        catch (Exception ex)
        {
            return new AuthenticationResponseDto
            {
                Errors = new List<string> {ex.Message},
                IsAuthenticated = false,
                Message = "Sorry, some problem occurred while authenticating you. Please try again.",
                Succeeded = false
            };
        }
    }
}
