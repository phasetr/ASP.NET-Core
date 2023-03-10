using IdentityByController.Data;

namespace IdentityByController.Models;

public class EfStoreRepository : IStoreRepository
{
    private readonly IdContext _context;

    public EfStoreRepository(IdContext ctx)
    {
        _context = ctx;
    }

    public IQueryable<Product> Products => _context.Products;

    public void CreateProduct(Product p)
    {
        _context.Add(p);
        _context.SaveChanges();
    }

    public void DeleteProduct(Product p)
    {
        _context.Remove(p);
        _context.SaveChanges();
    }

    public void SaveProduct(Product p)
    {
        _context.SaveChanges();
    }
}