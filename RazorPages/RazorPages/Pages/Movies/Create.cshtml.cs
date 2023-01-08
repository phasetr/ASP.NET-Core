using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Movies;

public class CreateModel : PageModel
{
    private readonly MyDbContext _context;

    public CreateModel(MyDbContext context)
    {
        _context = context;
    }

    [BindProperty] public Movie Movie { get; set; } = default!;

    public IActionResult OnGet()
    {
        return Page();
    }


    // To protect from over-posting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        // if (!ModelState.IsValid || _context.Movie == null || Movie == null) return Page();
        if (!ModelState.IsValid) return Page();

        _context.Movie.Add(Movie);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}