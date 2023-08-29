using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorJwtAuth.WebApi.Service.Services;

public interface IJwtTokenService
{
    JwtSecurityToken GetJwtToken(IEnumerable<Claim> userClaims);
}
