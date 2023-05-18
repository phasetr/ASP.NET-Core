using EFCoreQuestionSO20230315.Models;

namespace Test.Unit.Fakes;

public class ApplicationDbContextFakeBuilder : IDisposable
{
    private readonly ApplicationDbContextFake _context = new();

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    public ApplicationDbContextFakeBuilder WithProduct()
    {
        _context.Products.Add(new Product
        {
            Id = 1,
            Name = "Product1",
            Price = 100,
            ShopId = 1
        });
        _context.Products.Add(new Product
        {
            Id = 2,
            Name = "Product2",
            Price = 200,
            ShopId = 1,
            IsDeleted = true
        });
        _context.Products.Add(new Product
        {
            Id = 3,
            Name = "Product3",
            Price = 300,
            ShopId = 1
        });
        return this;
    }

    public ApplicationDbContextFakeBuilder WithShop()
    {
        _context.Shops.Add(new Shop
        {
            Id = 1,
            Name = "Shop1"
        });
        return this;
    }

    public ApplicationDbContextFake Build()
    {
        _context.SaveChanges();
        return _context;
    }
}
