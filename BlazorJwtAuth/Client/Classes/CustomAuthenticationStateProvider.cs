using System.Security.Claims;
using System.Text.Json;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Classes;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;

    public CustomAuthenticationStateProvider(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public void StateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var tokenDto = await _tokenService.GetTokenAsync();
            // TODO：トークンが切れている場合はリフレッシュトークンを使ってトークンを再取得する
            // トークンをUTCで発行しているため現在時刻と比較するときはUTCで比較する
            var identity = string.IsNullOrEmpty(tokenDto.Token) || tokenDto.Expiration < DateTime.UtcNow
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(tokenDto.Token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs is null
            ? Enumerable.Empty<Claim>()
            : keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}
