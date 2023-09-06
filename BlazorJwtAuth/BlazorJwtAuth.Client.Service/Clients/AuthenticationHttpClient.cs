using System.Net;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Client.Service.Classes;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Clients;

public class AuthenticationHttpClient
{
    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;

    public AuthenticationHttpClient(
        CustomAuthenticationStateProvider customAuthenticationStateProvider,
        HttpClient http,
        ITokenService tokenService)
    {
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
        _http = http;
        _tokenService = tokenService;
    }

    public async Task<UserRegisterResultDto> RegisterUser(UserRegisterDto userRegisterDto, AppSettings appSettings)
    {
        try
        {
            var response = await _http.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/register", userRegisterDto);
            var result = await response.Content.ReadFromJsonAsync<UserRegisterResultDto>();
            if (result is null)
                return new UserRegisterResultDto
                {
                    Succeeded = false,
                    Errors = new List<string>
                    {
                        "Sorry, we were unable to register you at this time. Please try again shortly."
                    },
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Sorry, we were unable to register you at this time. Please try again shortly.",
                    Detail = "Unable to deserialize response from server."
                };
            return result;
        }
        catch (Exception ex)
        {
            return new UserRegisterResultDto
            {
                Message = ex.Message,
                Succeeded = false,
                Errors = new List<string>
                {
                    "Sorry, we were unable to register you at this time. Please try again shortly."
                }
            };
        }
    }

    public async Task<UserLoginResultDto> LoginUser(UserLoginDto userLoginDto, AppSettings appSettings)
    {
        try
        {
            var response = await _http.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/login", userLoginDto);
            var result = await response.Content.ReadFromJsonAsync<UserLoginResultDto>();
            if (result is null)
                return new UserLoginResultDto
                {
                    Succeeded = false,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Sorry, we were unable to register you at this time. Please try again shortly.",
                    Detail = "Unable to deserialize response from server."
                };
            await _tokenService.SetTokenAsync(result.Token);
            _customAuthenticationStateProvider.StateChanged();
            return result;
        }
        catch (Exception ex)
        {
            return new UserLoginResultDto
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
