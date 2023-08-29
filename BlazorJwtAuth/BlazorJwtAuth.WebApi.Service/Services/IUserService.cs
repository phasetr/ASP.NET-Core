using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.WebApi.Service.Services;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<AuthenticationResponse> GetTokenAsync(GetTokenRequest model);
    Task<string> AddRoleAsync(AddRoleModel model);
    Task<AuthenticationResponse> RefreshTokenAsync(string requestRefreshToken);
    Task<bool> RevokeTokenAsync(string token);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
}
