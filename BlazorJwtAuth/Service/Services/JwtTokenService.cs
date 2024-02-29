using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Services.Interfaces;

namespace Service.Services;

public class JwtTokenService(
    IConfiguration configuration,
    IPtDateTime dateTime) : IJwtTokenService
{
    public JwtSecurityToken GetJwtToken(IEnumerable<Claim> userClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty));
        var expiryInMinutes = Convert.ToInt32(configuration["Jwt:DurationInMinutes"]);

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            userClaims,
            expires: dateTime.UtcNow.AddMinutes(expiryInMinutes),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
