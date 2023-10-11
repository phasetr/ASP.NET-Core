using Common.Dto;

namespace Client.Services.Interfaces;

public interface ITokenHttpClientService
{
    Task<AuthenticationResponseDto> GetTokenAsync(HttpClient httpClient, GetTokenDto getTokenDto);

    Task<AuthenticationResponseDto> RefreshTokenAsync(HttpClient httpClient, RefreshTokenDto refreshTokenDto);
}
