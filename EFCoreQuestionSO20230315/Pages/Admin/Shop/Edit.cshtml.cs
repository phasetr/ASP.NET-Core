using System.ComponentModel.DataAnnotations;
using EFCoreQuestionSO20230315.Data;
using EFCoreQuestionSO20230315.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Pages.Admin.Shop;

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

    [BindProperty] [Required] public string Name { get; set; } = default!;

    [BindProperty] [Required] public List<string> PaymentMethods { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var shop = await _context.Shop
            .Include(m => m.PaymentMethods)
            .FirstOrDefaultAsync(m => m.Id == Id);
        if (shop == null) return NotFound();
        Name = shop.Name;
        PaymentMethods = shop.PaymentMethods.Select(m => m.Name).ToList();
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

        var newPaymentMethods = PaymentMethods
            .Where(paymentMethod => !string.IsNullOrEmpty(paymentMethod))
            .Select(paymentMethod =>
                new PaymentMethod {Name = paymentMethod, Shop = shop, ShopId = shop.Id}).ToList();

        var paymentMethods = _context.PaymentMethod.Where(m => m.ShopId == Id).ToList();
        _context.PaymentMethod.RemoveRange(paymentMethods);

        shop.PaymentMethods = new List<PaymentMethod>(newPaymentMethods);
        await _context.PaymentMethod.AddRangeAsync(newPaymentMethods);
        _context.Update(shop);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShopExists(Id))
                return NotFound();
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool ShopExists(int id)
    {
        return _context.Shop.Any(e => e.Id == id);
    }
}
