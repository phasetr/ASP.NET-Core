using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorJwtAuth.Common.Dto;

public class UserLoginDto
{
    [DefaultValue("user@secureapi.com")]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [DefaultValue("Pa$$w0rd.")]
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
