using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Authentication;

public class Request
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }
}
