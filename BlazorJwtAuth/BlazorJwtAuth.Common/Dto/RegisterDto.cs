using System.ComponentModel.DataAnnotations;

namespace BlazorJwtAuth.Common.Dto;

public class RegisterDto
{
    [Required] public string FirstName { get; set; } = default!;

    [Required] public string LastName { get; set; } = default!;

    [Required] public string Username { get; set; } = default!;

    [Required] public string Email { get; set; } = default!;

    [Required] public string Password { get; set; } = default!;
}
