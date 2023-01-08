using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Data;
using RazorPages.Models;

namespace RazorPages.Pages.Movies;

public class DetailsModel : PageModel
{
    private readonly MyDbContext _context;

    public DetailsModel(MyDbContext context)
    {
        _context = context;
    }

    public Movie Movie { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Movie == null) return NotFound();

        var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
            return NotFound();
        Movie = movie;
        return Page();
    }
}