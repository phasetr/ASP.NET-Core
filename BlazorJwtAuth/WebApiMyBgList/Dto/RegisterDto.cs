using System.ComponentModel.DataAnnotations;

namespace WebApiMyBgList.Dto;

public class RegisterDto
{
    [Required] [MaxLength(255)] public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    [Required] public string? Password { get; set; }
}
