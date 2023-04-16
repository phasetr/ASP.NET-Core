using BlazorWasmHosted.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmHosted.Server.Pages.Admin.Shop;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Models.Shop Shop { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var shop = await _context.Shops.FirstOrDefaultAsync(m => m.Id == id);
        if (shop == null) return NotFound();
        Shop = shop;
        return Page();
    }
}
