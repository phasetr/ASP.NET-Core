using System.ComponentModel.DataAnnotations;
using BlazorWasmHosted.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmHosted.Server.Pages.Admin.Shop;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    [Required]
    public int Id { get; set; }

    [BindProperty]
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var shop = await _context.Shops.FirstOrDefaultAsync(m => m.Id == Id);
        if (shop == null) return NotFound();
        Name = shop.Name;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var shop = new Models.Shop
        {
            Id = Id,
            Name = Name
        };
        _context.Update(shop);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShopExists(shop.Id)) return NotFound();
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool ShopExists(int id)
    {
        return _context.Shops.Any(e => e.Id == id);
    }
}
