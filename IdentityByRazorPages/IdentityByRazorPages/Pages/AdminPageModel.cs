using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityByRazorPages.Pages;

[Authorize(Roles = "Admins")]
public class AdminPageModel : PageModel
{
}