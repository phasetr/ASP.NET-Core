using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Movies;

public class DeleteModel : PageModel
{
    private readonly MyDbContext _context;

    public DeleteModel(MyDbContext context)
    {
        _context = context;
    }

    [BindProperty] public Movie Movie { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Movie == null) return NotFound();

        var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
            return NotFound();
        Movie = movie;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Movie == null) return NotFound();
        var movie = await _context.Movie.FindAsync(id);

        if (movie != null)
        {
            Movie = movie;
            _context.Movie.Remove(Movie);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}