using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ITokenHttpClientService
{
    Task<AuthenticationResponseDto> GetTokenAsync(AppSettings appSettings, HttpClient httpClient,
        GetTokenResponseDto getTokenResponseDto);

    Task<AuthenticationResponseDto> RefreshTokenAsync(AppSettings appSettings, HttpClient httpClient,
        RefreshTokenDto refreshTokenDto);
}
