using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface IAuthenticationHttpClientService
{
    Task<UserRegisterResultDto> RegisterUser(UserRegisterDto userRegisterDto, AppSettings appSettings);
    Task<UserLoginResultDto> LoginUser(UserLoginDto userLoginDto, AppSettings appSettings);
    Task LogoutUser();
}
