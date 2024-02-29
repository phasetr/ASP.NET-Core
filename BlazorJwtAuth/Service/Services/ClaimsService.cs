using System.Security.Claims;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Services.Interfaces;

namespace Service.Services;

public class ClaimsService(UserManager<ApplicationUser> userManager) : IClaimsService
{
    public async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser applicationUser)
    {
        List<Claim> userClaims =
        [
            new Claim(ClaimTypes.Name, applicationUser.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, applicationUser.Email ?? string.Empty)
        ];

        var userRoles = await userManager.GetRolesAsync(applicationUser);

        userClaims.AddRange(userRoles.Select(userRole =>
            new Claim(ClaimTypes.Role, userRole)));

        return userClaims;
    }
}
