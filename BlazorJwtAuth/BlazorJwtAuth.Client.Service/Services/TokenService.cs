using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Models;
using Microsoft.Extensions.Logging;

namespace BlazorJwtAuth.Client.Service.Services;

public class TokenService : ITokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TokenService> _logger;

    public TokenService(
        IHttpClientFactory httpClientFactory,
        ILogger<TokenService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<AuthenticationResponse> GetTokenAsync(GetTokenRequest tokenRequest)
    {
        var httpClient = _httpClientFactory.CreateClient();
        // TODO APIのURLルート取得
        var response =
            await httpClient.PostAsync("https://localhost:5500/User/token", JsonContent.Create(tokenRequest));
        var res = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        // TODO null処理対応
        return res!;
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string token, string refreshToken)
    {
        throw new NotImplementedException();
    }
}
