using EfCoreBlazorServerStatic.Models;
using EfCoreBlazorServerStatic.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EfCoreBlazorServerStatic.Pages.Articles;

public class CreateModel : PageModel
{
    private readonly IdContext _context;

    public CreateModel(IdContext context)
    {
        _context = context;
    }

    [BindProperty] public Article Article { get; set; } = default!;

    public IActionResult OnGet()
    {
        ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
        return Page();
    }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Articles.Add(Article);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}