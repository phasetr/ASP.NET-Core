using Blazored.LocalStorage;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services;

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorageService;

    public TokenService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<TokenDto> GetTokenAsync()
    {
        return await _localStorageService.GetItemAsync<TokenDto>("token");
    }

    public async Task RemoveTokenAsync()
    {
        await _localStorageService.RemoveItemAsync("token");
    }

    public async Task SetTokenAsync(TokenDto tokenDto)
    {
        await _localStorageService.SetItemAsync("token", tokenDto);
    }
}
