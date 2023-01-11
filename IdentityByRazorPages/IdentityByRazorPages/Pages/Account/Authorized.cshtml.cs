using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityByRazorPages.Pages.Account;

[Authorize]
public class NeedAuth : PageModel
{
    public void OnGet()
    {
    }
}