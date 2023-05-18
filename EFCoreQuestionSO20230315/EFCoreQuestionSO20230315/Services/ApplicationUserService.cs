using EFCoreQuestionSO20230315.Data;
using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Services;

public interface IApplicationUserService
{
    public Task<IList<ApplicationUser>> GetAllAsync();
    public Task<IList<Shop>> GetAssignedShopsByNameAsync(string userName);
}

public class ApplicationUserService : IApplicationUserService
{
    private readonly ApplicationDbContext _context;

    public ApplicationUserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<ApplicationUser>> GetAllAsync()
    {
        return await _context.ApplicationUsers
            .Include(x => x.ApplicationUserShops)
            .ThenInclude(x => x.Shop)
            .ToListAsync();
    }

    public async Task<IList<Shop>> GetAssignedShopsByNameAsync(string userName)
    {
        return await _context.ApplicationUsers
            .Include(x => x.ApplicationUserShops)
            .ThenInclude(x => x.Shop)
            .Where(x => x.UserName == userName)
            .SelectMany(x => x.ApplicationUserShops.Select(a => a.Shop))
            .ToListAsync();
    }
}
