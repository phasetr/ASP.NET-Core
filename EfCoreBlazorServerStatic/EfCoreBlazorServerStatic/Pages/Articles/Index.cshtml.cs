using EfCoreBlazorServerStatic.Models;
using EfCoreBlazorServerStatic.Models.Contexts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBlazorServerStatic.Pages.Articles;

public class IndexModel : PageModel
{
    private readonly IdContext _context;

    public IndexModel(IdContext context)
    {
        _context = context;
    }

    public IList<Article> Article { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Article = await _context.Articles
            .Include(a => a.User).ToListAsync();
    }
}