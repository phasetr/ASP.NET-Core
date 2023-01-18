using CityBreaks.Data;
using CityBreaks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CityBreaks.Pages.PropertyManager;

public class CreateModel : PageModel
{
    private readonly CityBreaksContext _context;

    public CreateModel(CityBreaksContext context)
    {
        _context = context;
    }

    [BindProperty] public Property Property { get; set; }

    public IActionResult OnGet()
    {
        ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id");
        return Page();
    }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Properties.Add(Property);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}