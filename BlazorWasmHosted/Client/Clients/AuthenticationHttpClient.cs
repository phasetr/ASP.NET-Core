using System.Net.Http.Json;
using BlazorWasmHosted.Client.Services;
using BlazorWasmHosted.Shared.DTOs;

namespace BlazorWasmHosted.Client.Clients;

public class AuthenticationHttpClient
{
    private readonly HttpClient _http;
    private readonly ILogger<AuthenticationHttpClient> _logger;
    private readonly ITokenService _tokenService;
    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;

    public AuthenticationHttpClient(ILogger<AuthenticationHttpClient> logger,
        HttpClient http,
        ITokenService tokenService,
        CustomAuthenticationStateProvider customAuthenticationStateProvider)
    {
        _logger = logger;
        _http = http;
        _tokenService = tokenService;
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
    }

    public async Task<UserRegisterResultDto?> RegisterUser(UserRegisterDto userRegisterDto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("user/register", userRegisterDto);
            var result = await response.Content.ReadFromJsonAsync<UserRegisterResultDto>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);

            return new UserRegisterResultDto
            {
                Succeeded = false,
                Errors = new List<string>
                {
                    "Sorry, we were unable to register you at this time. Please try again shortly."
                }
            };
        }
    }

    public async Task<UserLoginResultDto?> LoginUser(UserLoginDto userLoginDto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("user/login", userLoginDto);
            var result = await response.Content.ReadFromJsonAsync<UserLoginResultDto>();
            if (result?.Succeeded == true) await _tokenService.SetToken(result.Token);
            _customAuthenticationStateProvider.StateChanged();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);

            return new UserLoginResultDto
            {
                Succeeded = false,
                Message = "Sorry, we were unable to log you in at this time. Please try again shortly."
            };
        }
    }
    public async Task LogoutUser()
    {
        await _tokenService.RemoveToken();
        _customAuthenticationStateProvider.StateChanged();
    }
}
