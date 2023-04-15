using System.Text.Json.Serialization;

namespace WebApi.Models;

public class ApiUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    [JsonIgnore] public string PasswordHash { get; set; }

    [JsonIgnore] public List<RefreshToken> RefreshTokens { get; set; }
}
