using EfCoreBlazorServerStatic.Models;
using EfCoreBlazorServerStatic.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBlazorServerStatic.Pages.Articles;

public class DeleteModel : PageModel
{
    private readonly IdContext _context;

    public DeleteModel(IdContext context)
    {
        _context = context;
    }

    [BindProperty] public Article Article { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.Articles.FirstOrDefaultAsync(m => m.ArticleId == id);

        if (article == null)
            return NotFound();
        Article = article;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();
        var article = await _context.Articles.FindAsync(id);

        if (article != null)
        {
            Article = article;
            _context.Articles.Remove(Article);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}