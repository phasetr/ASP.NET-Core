using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ITokenService
{
    Task<TokenDto> GetTokenAsync();
    Task RemoveTokenAsync();
    Task SetTokenAsync(TokenDto tokenDto);
}
