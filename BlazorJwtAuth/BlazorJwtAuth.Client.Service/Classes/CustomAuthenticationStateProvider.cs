using System.Security.Claims;
using System.Text.Json;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BlazorJwtAuth.Client.Service.Classes;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;
    private readonly ITokenService _tokenService;

    public CustomAuthenticationStateProvider(
        ILogger<CustomAuthenticationStateProvider> logger,
        ITokenService tokenService)
    {
        _logger = logger;
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
            var tokenDto = await _tokenService.GetToken();
            var identity = string.IsNullOrEmpty(tokenDto.Token) || tokenDto.Expiration < DateTime.Now
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(tokenDto.Token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
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
