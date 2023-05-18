using EFCoreQuestionSO20230315.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Pages.Admin.Shop;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Models.Shop> Shop { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Shop = await _context.Shops
            .OrderBy(m => m.Id)
            .ToListAsync();
    }
}
