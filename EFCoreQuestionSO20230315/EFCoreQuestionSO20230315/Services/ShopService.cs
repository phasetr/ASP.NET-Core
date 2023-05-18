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
            .Include(s =>
                s.Products.Where(p => !p.IsDeleted))
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
