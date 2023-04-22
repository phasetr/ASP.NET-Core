using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlazorWasmHosted.Server.Models;
using BlazorWasmHosted.Server.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BlazorWasmHosted.Server.EndPoints;

public static class Security
{
    public static void MapSecurity(this WebApplication app)
    {
        app.MapPost("/security/createToken",
            [AllowAnonymous] async (CreateTokenData createTokenData, UserManager<ApplicationUser> userManager) =>
            {
                var user = await userManager.FindByNameAsync(createTokenData.UserName);
                if (user is null) return Results.Unauthorized();
                var isValidPassword = await userManager.CheckPasswordAsync(user, createTokenData.Password);
                if (!isValidPassword) return Results.Unauthorized();
                var issuer = app.Configuration["Jwt:Issuer"];
                var audience = app.Configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(app.Configuration["Jwt:Key"] ?? string.Empty);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(100),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                return Results.Ok(jwtToken);
            });

        app.MapGet("/security/getMessage",
            () => "Hello World!").RequireAuthorization();
    }
}
