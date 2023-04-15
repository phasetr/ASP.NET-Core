using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users;

public class AuthenticateRequest
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }
}
