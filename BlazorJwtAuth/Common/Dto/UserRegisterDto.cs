using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Dto;

public class UserRegisterDto
{
    [DefaultValue("user1@secureapi.com")]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [DefaultValue("user1password")]
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DefaultValue("user1password")]
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string? ConfirmPassword { get; set; }

    [DefaultValue("first user1")] public string? FirstName { get; set; }

    [DefaultValue("last user1")] public string? LastName { get; set; }

    [DefaultValue("user1name")] public string? Username { get; set; }
}
