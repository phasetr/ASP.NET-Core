using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Authentication;

namespace WebApi.Services.Authorization;

public interface IJwtUtils
{
    /// <summary>
    ///     JWTトークンを生成する。
    /// </summary>
    /// <param name="applicationUser">ユーザーオブジェクト</param>
    /// <returns>JWTトークン</returns>
    public string GenerateJwtToken(ApplicationUser applicationUser);

    /// <summary>
    ///     トークンを検証してトークンが有効な場合はユーザーIDを返す。
    /// </summary>
    /// <param name="token">JWTトークン</param>
    /// <returns>認証されたらユーザーID。認証失敗時はnull</returns>
    public string ValidateJwtToken(string token);

    /// <summary>
    ///     データベースにアクセスして既存のトークンと衝突しないようにトークンを生成する.
    /// </summary>
    /// <param name="ipAddress">トークンに記録するためのIPアドレス</param>
    /// <returns>リフレッシュトークン</returns>
    public RefreshToken GenerateRefreshToken(string ipAddress);
}

public class JwtUtils : IJwtUtils
{
    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;

    public JwtUtils(
        ApplicationDbContext context,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public string GenerateJwtToken(ApplicationUser applicationUser)
    {
        // generate token that is valid for 15 minutes
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {new Claim("id", applicationUser.Id)}),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string ValidateJwtToken(string token)
    {
        if (token == null) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set ClockSkew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

            // return applicationUser id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = GetUniqueToken(),
            // token is valid for 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        return refreshToken;

        string GetUniqueToken()
        {
            while (true)
            {
                // token is a cryptographically strong random sequence of values
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                // ensure token is unique by checking against db
                var tokenIsUnique = !_context.ApplicationUsers
                    .Any(u => u.RefreshTokens.Any(t => t.Token == token));

                if (tokenIsUnique) return token;
            }
        }
    }
}
