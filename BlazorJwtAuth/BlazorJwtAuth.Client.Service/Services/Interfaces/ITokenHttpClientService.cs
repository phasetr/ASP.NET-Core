using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ITokenHttpClientService
{
    Task<AuthenticationResponseDto> GetTokenAsync(HttpClient httpClient, GetTokenResponseDto getTokenResponseDto);

    Task<AuthenticationResponseDto> RefreshTokenAsync(HttpClient httpClient, RefreshTokenDto refreshTokenDto);
}
