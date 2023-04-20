using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BlazorWasmHosted.Server.Services;

public interface IJwtTokenService
{
    JwtSecurityToken GetJwtToken(IEnumerable<Claim> userClaims);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JwtSecurityToken GetJwtToken(IEnumerable<Claim> userClaims)
    {
        var authSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? string.Empty));
        var expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

        var token = new JwtSecurityToken(
            _configuration["Jwt:ValidIssuer"],
            _configuration["Jwt:ValidAudience"],
            userClaims,
            expires: DateTime.Now.AddMinutes(expiryInMinutes),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
