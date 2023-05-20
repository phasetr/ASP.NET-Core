using EFCoreQuestionSO20230315.Data;
using EFCoreQuestionSO20230315.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EFCoreQuestionSO20230315.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public Shop? Shop { get; set; }

    public void OnGet()
    {
        var shops = (from s in _context.Shops.AsEnumerable()
                join p in _context.Products.Where(x => x.IsDeleted == false) on s.Id equals p.ShopId
                select new {s, p})
            .GroupBy(x => x.s)
            .Select(x => new Shop
            {
                Id = x.Key.Id,
                Name = x.Key.Name,
                Products = x.Select(y => y.p).ToList()
            })
            .ToList();
        if (shops.Count > 0) Shop = shops[0];
        Console.WriteLine("TEST");
    }
}
