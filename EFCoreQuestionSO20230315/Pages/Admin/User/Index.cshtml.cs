using EFCoreQuestionSO20230315.Data;
using EFCoreQuestionSO20230315.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Pages.Admin.User;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ApplicationUser> ApplicationUsers { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.ApplicationUser != null)
            ApplicationUsers = await _context.ApplicationUser
                .Include(m => m.ApplicationRoles)
                .ToListAsync();
    }
}
