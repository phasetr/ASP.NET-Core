using EfCoreBlazorServerStatic.Models;
using EfCoreBlazorServerStatic.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBlazorServerStatic.Pages.Articles;

public class EditModel : PageModel
{
    private readonly IdContext _context;

    public EditModel(IdContext context)
    {
        _context = context;
    }

    [BindProperty] public Article Article { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var article = await _context.Articles.FirstOrDefaultAsync(m => m.ArticleId == id);
        if (article == null) return NotFound();
        Article = article;
        ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Attach(Article).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ArticleExists(Article.ArticleId))
                return NotFound();
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool ArticleExists(int id)
    {
        return _context.Articles.Any(e => e.ArticleId == id);
    }
}