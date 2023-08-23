using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorJwtAuth.Common.Models;

public class TokenRequestModel
{
    [DefaultValue("user@secureapi.com")]
    [Required]
    public string Email { get; set; } = default!;

    [DefaultValue("Pa$$w0rd.")] [Required] public string Password { get; set; } = default!;
}
