using BlazorWasmHosted.Server.Data;
using BlazorWasmHosted.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmHosted.Server.Services;

public interface IShopService
{
    public Task<IList<Shop>> GetAllAsync();
}

public class ShopService : IShopService
{
    private readonly ApplicationDbContext _context;

    public ShopService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Shop>> GetAllAsync()
    {
        return await _context.Shops.ToListAsync();
    }
}
