using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Models;

[Index(nameof(UserName), IsUnique = true)]
public class ApplicationUser : IdentityUser
{
    [Required] public override string UserName { get; set; } = default!;

    public ICollection<ApplicationRole>? ApplicationRoles { get; set; }
    public ICollection<ApplicationUserShop> ApplicationUserShops { get; set; } = default!;
}
