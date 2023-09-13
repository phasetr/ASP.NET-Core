using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.EntityModels.Entities;

namespace BlazorJwtAuth.WebApi.Service.Services.Interfaces;

public interface IUserService
{
    Task<UserRegisterResponseDto> RegisterAsync(UserRegisterDto dto);
    Task<AuthenticationResponseDto> GetTokenAsync(GetTokenResponseDto model);
    Task<string> AddRoleAsync(AddRoleDto dto);
    Task<AuthenticationResponseDto> RefreshTokenAsync(string requestRefreshToken);
    Task<bool> RevokeTokenAsync(string token);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
}
