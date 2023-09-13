using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface IAuthenticationHttpClientService
{
    Task<UserRegisterResponseDto> RegisterUser(HttpClient httpClient, UserRegisterDto userRegisterDto);
    Task<UserLoginResponseDto> LoginUser(HttpClient httpClient, UserLoginDto userLoginDto);
    Task LogoutUser();
}
