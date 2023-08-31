using System.Net.Http.Json;
using Blazored.LocalStorage;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Services;

public class TokenService : ITokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorageService;

    public TokenService(
        IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorageService)
    {
        _httpClientFactory = httpClientFactory;
        _localStorageService = localStorageService;
    }

    public async Task<AuthenticationResponse> GetTokenAsync(GetTokenRequest tokenRequest)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var appSettings = await _localStorageService.GetItemAsync<AppSettings>("appSettings");
        var response =
            await httpClient.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/token", tokenRequest);
        var res = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        if (res is null)
            return new AuthenticationResponse
            {
                Status = Response.ErrorResponseStatus,
                Message = Response.ErrorResponseMessage,
                Detail = Response.ErrorResponseDetail
            };
        return res;
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string token, string refreshToken)
    {
        // TODO：実装
        await Task.Delay(1);
        throw new NotImplementedException();
    }

    public async Task<TokenDto> GetToken()
    {
        return await _localStorageService.GetItemAsync<TokenDto>("token");
    }

    public async Task RemoveToken()
    {
        await _localStorageService.RemoveItemAsync("token");
    }

    public async Task SetToken(TokenDto tokenDto)
    {
        await _localStorageService.SetItemAsync("token", tokenDto);
    }
}
