using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    [JsonIgnore] public override string PasswordHash { get; set; }

    [JsonIgnore] public List<RefreshToken> RefreshTokens { get; set; } = default!;
}
