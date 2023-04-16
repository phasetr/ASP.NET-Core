using BlazorWasmHosted.Server.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorWasmHosted.Server.Pages.Admin.Shop;

public class IndexModel : PageModel
{
    // private readonly ApplicationDbContext _context;
    private readonly IShopService _shopService;

    public IndexModel(IShopService shopService)
    {
        _shopService = shopService;
    }

    public IList<Models.Shop> Shop { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Shop = await _shopService.GetAllAsync();
    }
}
