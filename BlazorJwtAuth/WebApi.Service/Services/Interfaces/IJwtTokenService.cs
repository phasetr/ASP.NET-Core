using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Service.Services.Interfaces;

public interface IJwtTokenService
{
    /// <summary>
    ///     appsettings.jsonから取得したJWT情報をもとにJWTトークンを生成する。
    /// </summary>
    /// <param name="userClaims">ユーザークレームのリスト</param>
    /// <returns>JWTトークン</returns>
    JwtSecurityToken GetJwtToken(IEnumerable<Claim> userClaims);
}
