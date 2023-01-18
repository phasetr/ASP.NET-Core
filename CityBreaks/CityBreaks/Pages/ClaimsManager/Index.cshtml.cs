using CityBreaks.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CityBreaks.Pages.ClaimsManager;

public class IndexModel : PageModel
{
    public IndexModel(UserManager<CityBreaksUser> userManager)
    {
        UserManager = userManager;
    }

    public UserManager<CityBreaksUser> UserManager { get; set; }
    public List<CityBreaksUser> Users { get; set; }

    public async Task OnGetAsync()
    {
        Users = await UserManager.Users.ToListAsync();
    }
}