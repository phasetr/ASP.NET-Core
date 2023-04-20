using Microsoft.AspNetCore.Identity;

namespace BlazorWasmHosted.Server.Models;

public class ApplicationRole : IdentityRole<int>
{
}

public enum UserRoles
{
    Admin,
    Staff,
    Customer
}
