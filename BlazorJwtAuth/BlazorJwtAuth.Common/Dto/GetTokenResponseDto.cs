using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorJwtAuth.Common.Dto;

public class GetTokenResponseDto
{
    [DefaultValue("user@secureapi.com")]
    [Required]
    public string Email { get; init; } = default!;

    [DefaultValue("Pa$$w0rd.")] [Required] public string Password { get; init; } = default!;
}
