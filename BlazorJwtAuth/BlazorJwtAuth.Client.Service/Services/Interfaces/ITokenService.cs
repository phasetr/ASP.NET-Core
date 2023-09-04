using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ITokenService
{
    Task<TokenDto> GetToken();
    Task RemoveToken();
    Task SetToken(TokenDto tokenDto);
}
