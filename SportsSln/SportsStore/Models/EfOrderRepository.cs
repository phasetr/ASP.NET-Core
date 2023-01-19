using Microsoft.EntityFrameworkCore;
using SportsStore.Data;

namespace SportsStore.Models;

public class EfOrderRepository : IOrderRepository
{
    private readonly IdDbContext _context;

    public EfOrderRepository(IdDbContext ctx)
    {
        _context = ctx;
    }

    public IQueryable<Order> Orders => _context.Orders
        .Include(o => o.Lines)
        .ThenInclude(l => l.Product);

    public void SaveOrder(Order order)
    {
        _context.AttachRange(order.Lines.Select(l => l.Product));
        if (order.OrderID == 0) _context.Orders.Add(order);
        _context.SaveChanges();
    }
}