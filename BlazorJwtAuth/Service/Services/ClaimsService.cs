using System.Security.Claims;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Services.Interfaces;

namespace Service.Services;

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
