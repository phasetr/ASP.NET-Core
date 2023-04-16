using BlazorWasmHosted.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmHosted.Server.Pages.Admin.Shop;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public Models.Shop Shop { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var shop = await _context.Shops.FirstOrDefaultAsync(m => m.Id == id);

        if (shop == null)
            return NotFound();
        Shop = shop;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();
        var shop = await _context.Shops.FindAsync(id);

        if (shop == null) return RedirectToPage("./Index");
        Shop = shop;
        _context.Shops.Remove(Shop);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
