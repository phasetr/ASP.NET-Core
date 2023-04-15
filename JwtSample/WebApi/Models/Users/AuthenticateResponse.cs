using System.Text.Json.Serialization;

namespace WebApi.Models.Users;

public class AuthenticateResponse
{
    public AuthenticateResponse(ApplicationUser applicationUser, string jwtToken, string refreshToken)
    {
        FirstName = applicationUser.FirstName;
        LastName = applicationUser.LastName;
        UserName = applicationUser.UserName;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
}
