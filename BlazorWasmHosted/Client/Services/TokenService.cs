using Blazored.LocalStorage;
using BlazorWasmHosted.Shared.DTOs;

namespace BlazorWasmHosted.Client.Services;

public interface ITokenService
{
    Task<TokenDto> GetToken();
    Task RemoveToken();
    Task SetToken(TokenDto tokenDto);
}

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorageService;

    public TokenService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task SetToken(TokenDto tokenDto)
    {
        await _localStorageService.SetItemAsync("token", tokenDto.Token);
    }

    public async Task<TokenDto> GetToken()
    {
        return await _localStorageService.GetItemAsync<TokenDto>("token");
    }

    public async Task RemoveToken()
    {
        await _localStorageService.RemoveItemAsync("token");
    }
}
