using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ITokenService
{
    Task<AuthenticationResponse> GetTokenAsync(GetTokenRequest model);
    Task<AuthenticationResponse> RefreshTokenAsync(string token, string refreshToken);
    Task<TokenDto> GetToken();
    Task RemoveToken();
    Task SetToken(TokenDto tokenDto);
}
