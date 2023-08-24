using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.WebApi.Service.Services;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
    Task<string> AddRoleAsync(AddRoleModel model);
    Task<AuthenticationModel> RefreshTokenAsync(string jwtToken);
    Task<bool> RevokeTokenAsync(string token);
    Task<ApplicationUser?> GetByIdAsync(string id);
}
