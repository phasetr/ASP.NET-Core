using System.Text.Json.Serialization;

namespace WebApi.Models.Users;

public class AuthenticateResponse
{
    public AuthenticateResponse(ApiUser apiUser, string jwtToken, string refreshToken)
    {
        FirstName = apiUser.FirstName;
        LastName = apiUser.LastName;
        UserName = apiUser.UserName;
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
