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
        return await (from s in _context.Shops
                join p in _context.Products.Where(x => x.IsDeleted == false) on s.Id equals p.ShopId
                select new {s, p})
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
