using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorJwtAuth.Common.Models;

public class GetTokenRequest
{
    [DefaultValue("user@secureapi.com")]
    [Required]
    public string Email { get; init; } = default!;

    [DefaultValue("Pa$$w0rd.")] [Required] public string Password { get; init; } = default!;
}
