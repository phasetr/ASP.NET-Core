using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services;

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
                    Detail = "Unable to deserialize response from server.",
                    IsAuthenticated = false,
                    Message = "Sorry, we were unable to refresh a token. Please try again shortly."
                };

            return new AuthenticationResponseDto
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
            return new AuthenticationResponseDto
            {
                Detail = ex.Message,
                IsAuthenticated = false,
                Message = "Sorry, some problem occurred while authenticating you. Please try again."
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
                    Detail = "Unable to deserialize response from server.",
                    IsAuthenticated = false,
                    Message = "Sorry, we were unable to authenticate you at this time. Please try again shortly."
                };

            return new AuthenticationResponseDto
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
            return new AuthenticationResponseDto
            {
                Detail = ex.Message,
                IsAuthenticated = false,
                Message = "Sorry, some problem occurred while authenticating you. Please try again."
            };
        }
    }
}
