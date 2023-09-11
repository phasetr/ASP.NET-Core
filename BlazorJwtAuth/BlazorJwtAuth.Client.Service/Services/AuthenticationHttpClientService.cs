using System.Net;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Classes;
using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services;

public class AuthenticationHttpClientService : IAuthenticationHttpClientService
{
    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenService _tokenService;

    public AuthenticationHttpClientService(
        CustomAuthenticationStateProvider customAuthenticationStateProvider,
        IHttpClientFactory httpClientFactory,
        ITokenService tokenService)
    {
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
    }

    public async Task<UserRegisterResultDto> RegisterUser(UserRegisterDto userRegisterDto, AppSettings appSettings)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/register", userRegisterDto);
            var result = await response.Content.ReadFromJsonAsync<UserRegisterResultDto>();
            if (result is null)
                return new UserRegisterResultDto
                {
                    Detail = "Unable to deserialize response from server.",
                    Errors = new List<string>
                    {
                        "Sorry, we were unable to register you at this time. Please try again shortly."
                    },
                    Message = "Sorry, we were unable to register you at this time. Please try again shortly.",
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Succeeded = false
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
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{appSettings.ApiBaseAddress}/User/login", userLoginDto);
            var result = await response.Content.ReadFromJsonAsync<UserLoginResultDto>();
            if (result is null)
                return new UserLoginResultDto
                {
                    Detail = "Unable to deserialize response from server.",
                    Message = "Sorry, we were unable to register you at this time. Please try again shortly.",
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Succeeded = false
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