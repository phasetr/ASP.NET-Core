using System.ComponentModel.DataAnnotations;

namespace BlazorWebAssemblyWithRazorPages.Server.Models.Users;

public class AuthenticateRequest
{
    [Required] public string UserName { get; set; } = default!;

    [Required] public string Password { get; set; } = default!;
}
