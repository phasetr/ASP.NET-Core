using System.Net.Http.Json;
using Client.Classes;
using Client.Services.Interfaces;
using Common.Constants;
using Common.Dto;

namespace Client.Services;

public class AuthenticationHttpClientService : IAuthenticationHttpClientService
{
    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
    private readonly ITokenService _tokenService;

    public AuthenticationHttpClientService(
        CustomAuthenticationStateProvider customAuthenticationStateProvider,
        ITokenService tokenService)
    {
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
        _tokenService = tokenService;
    }

    public async Task<UserRegisterResponseDto> RegisterUser(HttpClient httpClient, UserRegisterDto userRegisterDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(ApiPath.V1UserRegisterFull,
                userRegisterDto);
            var result = await response.Content.ReadFromJsonAsync<UserRegisterResponseDto>();
            if (result is null)
                return new UserRegisterResponseDto
                {
                    Errors = new List<string>
                    {
                        "Sorry, we were unable to register you at this time. Please try again shortly."
                    },
                    Message = "Sorry, we were unable to register you at this time. Please try again shortly.",
                    Succeeded = false
                };
            return result;
        }
        catch (Exception ex)
        {
            return new UserRegisterResponseDto
            {
                Message = ex.Message,
                Succeeded = false,
                Errors = new List<string>
                {
                    "Sorry, we were unable to register you at this time. Please try again shortly.",
                    ex.Message
                }
            };
        }
    }

    public async Task<UserLoginResponseDto> LoginUser(HttpClient httpClient, UserLoginDto userLoginDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(ApiPath.V1UserLoginFull, userLoginDto);
            var result = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            if (result is null)
                return new UserLoginResponseDto
                {
                    Message = "Sorry, we were unable to register you at this time. Please try again shortly.",
                    Succeeded = false
                };
            await _tokenService.SetTokenAsync(result.Token);
            _customAuthenticationStateProvider.StateChanged();
            return result;
        }
        catch (Exception ex)
        {
            return new UserLoginResponseDto
            {
                Message = ex.Message,
                Succeeded = false
            };
        }
    }

    public async Task LogoutUser()
    {
        await _tokenService.RemoveTokenAsync();
        _customAuthenticationStateProvider.StateChanged();
    }
}
