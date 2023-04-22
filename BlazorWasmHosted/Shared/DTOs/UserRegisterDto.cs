using System.ComponentModel.DataAnnotations;

namespace BlazorWasmHosted.Shared.DTOs;

public class UserRegisterDto
{
    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string? ConfirmPassword { get; set; }
}
