using Microsoft.AspNetCore.Identity;

namespace BlazorWasmHosted.Server.Models;

public sealed class ApplicationRole : IdentityRole
{
}

public static class UserRoles
{
    public const string Admin = nameof(Admin);
    public const string User = nameof(User);
}
