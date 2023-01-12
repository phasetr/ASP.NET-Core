using Microsoft.AspNetCore.Identity;

namespace EfCoreBlazorServerStatic.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public virtual IEnumerable<Article>? Articles { get; set; }
}