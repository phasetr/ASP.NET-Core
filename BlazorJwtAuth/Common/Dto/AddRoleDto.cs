using System.ComponentModel.DataAnnotations;

namespace Common.Dto;

public class AddRoleDto
{
    [Required] public string Email { get; set; } = default!;

    [Required] public string Password { get; set; } = default!;

    [Required] public string Role { get; set; } = default!;
}
