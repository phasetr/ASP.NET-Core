using EFCoreQuestionSO20230315.Data;
using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Services;

public interface IShopService
{
    Task<Shop?> GetShopWithProductsByIdAsync(int id);
}

public class ShopService : IShopService
{
    private readonly ApplicationDbContext _context;

    public ShopService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Shop?> GetShopWithProductsByIdAsync(int id)
    {
        return await _context.Shops
            .Join(_context.Products.Where(p => !p.IsDeleted),
                s => s.Id,
                p => p.ShopId,
                (s, p) => new {s, p})
            .GroupBy(x => x.s)
            .Select(x => new Shop
            {
                Id = x.Key.Id,
                Name = x.Key.Name,
                Products = x.Select(y => y.p).ToList()
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
