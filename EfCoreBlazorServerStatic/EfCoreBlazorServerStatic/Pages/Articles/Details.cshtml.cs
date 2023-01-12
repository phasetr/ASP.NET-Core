using EfCoreBlazorServerStatic.Models;
using EfCoreBlazorServerStatic.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBlazorServerStatic.Pages.Articles;

public class DetailsModel : PageModel
{
    private readonly IdContext _context;

    public DetailsModel(IdContext context)
    {
        _context = context;
    }

    public Article Article { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.Articles.FirstOrDefaultAsync(m => m.ArticleId == id);
        if (article == null) return NotFound();
        Article = article;
        return Page();
    }
}