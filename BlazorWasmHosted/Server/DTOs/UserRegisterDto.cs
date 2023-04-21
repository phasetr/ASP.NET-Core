using System.ComponentModel.DataAnnotations;

namespace BlazorWasmHosted.Server.DTOs;

public class UserRegisterDto
{
    [Microsoft.Build.Framework.Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Microsoft.Build.Framework.Required] public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string? ConfirmPassword { get; set; }
}
