using System.Text.Json.Serialization;

namespace WebApi.Models.Users;

public class AuthenticateResponse
{
    public AuthenticateResponse(User user, string jwtToken, string refreshToken)
    {
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
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
