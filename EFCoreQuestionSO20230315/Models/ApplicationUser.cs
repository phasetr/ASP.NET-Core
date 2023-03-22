using Microsoft.AspNetCore.Identity;

namespace EFCoreQuestionSO20230315.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<ApplicationRole>? ApplicationRoles { get; set; }
}
