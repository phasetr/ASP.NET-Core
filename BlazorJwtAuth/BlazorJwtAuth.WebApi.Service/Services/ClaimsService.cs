using System.Security.Claims;
using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.WebApi.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BlazorJwtAuth.WebApi.Service.Services;

public class ClaimsService : IClaimsService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ClaimsService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser applicationUser)
    {
        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.Name, applicationUser.UserName),
            new Claim(ClaimTypes.Email, applicationUser.Email)
        };

        var userRoles = await _userManager.GetRolesAsync(applicationUser);

        userClaims.AddRange(userRoles.Select(userRole =>
            new Claim(ClaimTypes.Role, userRole)));

        return userClaims;
    }
}
