using Common.Dto;
using Common.EntityModels.Entities;

namespace Service.Services.Interfaces;

public interface IUserService
{
    Task<UserRegisterResponseDto> RegisterAsync(UserRegisterDto dto);
    Task<AuthenticationResponseDto> GetTokenAsync(GetTokenDto model);
    Task<string> AddRoleAsync(AddRoleDto dto);
    Task<AuthenticationResponseDto> RefreshTokenAsync(string requestRefreshToken);
    Task<bool> RevokeTokenAsync(string token);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> FindByEmailAsync(string email);
}
