using System.ComponentModel.DataAnnotations;

namespace BlazorJwtAuth.Common.Models;

public class AddRoleModel
{
    [Required] public string Email { get; set; } = default!;

    [Required] public string Password { get; set; } = default!;

    [Required] public string Role { get; set; } = default!;
}
