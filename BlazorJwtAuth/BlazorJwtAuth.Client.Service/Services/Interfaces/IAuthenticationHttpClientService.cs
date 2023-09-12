using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface IAuthenticationHttpClientService
{
    Task<UserRegisterResponseDto> RegisterUser(UserRegisterDto userRegisterDto, AppSettings appSettings);
    Task<UserLoginResponseDto> LoginUser(HttpClient httpClient, UserLoginDto userLoginDto);
    Task LogoutUser();
}
