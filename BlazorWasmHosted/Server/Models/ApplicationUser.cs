using Microsoft.AspNetCore.Identity;

namespace BlazorWasmHosted.Server.Models;

public class ApplicationUser : IdentityUser<int>
{
    public override int Id { get; set; }
}
