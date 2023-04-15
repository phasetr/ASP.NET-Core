using System.Text.Json.Serialization;

namespace WebApi.Models.Users;

public class AuthenticateResponse
{
    public AuthenticateResponse(ApiUser apiUser, string jwtToken, string refreshToken)
    {
        FirstName = apiUser.FirstName;
        LastName = apiUser.LastName;
        Username = apiUser.Username;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
}
