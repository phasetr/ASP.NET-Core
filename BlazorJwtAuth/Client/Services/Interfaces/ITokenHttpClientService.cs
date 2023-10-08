using Common.Dto;

namespace Client.Services.Interfaces;

public interface ITokenHttpClientService
{
    Task<AuthenticationResponseDto> GetTokenAsync(HttpClient httpClient, GetTokenResponseDto getTokenResponseDto);

    Task<AuthenticationResponseDto> RefreshTokenAsync(HttpClient httpClient, RefreshTokenDto refreshTokenDto);
}
