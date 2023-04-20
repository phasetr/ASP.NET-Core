using System.ComponentModel.DataAnnotations;

namespace BlazorWasmHosted.Server.DTOs;

public class UserRegisterDto
{
    [Microsoft.Build.Framework.Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Microsoft.Build.Framework.Required] public string? Password { get; set; }
}
