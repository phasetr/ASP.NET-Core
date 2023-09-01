using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlazorJwtAuth.WebApi.Service.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlazorJwtAuth.WebApi.Service.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IPtDateTime _dateTime;

    public JwtTokenService(
        IConfiguration configuration,
        IPtDateTime dateTime)
    {
        _configuration = configuration;
        _dateTime = dateTime;
    }

    public JwtSecurityToken GetJwtToken(IEnumerable<Claim> userClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var expiryInMinutes = Convert.ToInt32(_configuration["Jwt:DurationInMinutes"]);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            userClaims,
            expires: _dateTime.UtcNow.AddMinutes(expiryInMinutes),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
