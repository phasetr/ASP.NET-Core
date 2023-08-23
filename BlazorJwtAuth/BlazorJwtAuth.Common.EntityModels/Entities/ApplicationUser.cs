using Microsoft.AspNetCore.Identity;

namespace BlazorJwtAuth.Common.EntityModels.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public List<RefreshToken> RefreshTokens { get; set; } = default!;
}
