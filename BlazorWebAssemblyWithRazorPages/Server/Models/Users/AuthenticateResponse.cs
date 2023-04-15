using System.Text.Json.Serialization;

namespace BlazorWebAssemblyWithRazorPages.Server.Models.Users;

public class AuthenticateResponse
{
    public AuthenticateResponse(ApplicationUser user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        UserName = user.UserName;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
}
