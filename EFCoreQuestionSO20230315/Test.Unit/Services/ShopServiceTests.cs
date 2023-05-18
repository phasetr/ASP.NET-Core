using EFCoreQuestionSO20230315.Services;
using Test.Unit.Fakes;

namespace Test.Unit.Services;

public class ShopServiceTests:IDisposable
{
    private readonly ApplicationDbContextFakeBuilder _contextFakeBuilder = new();
    
    private ShopService _sut = default!;

    public void Dispose()
    {
        _contextFakeBuilder.Dispose();
    }
    
    [Fact]
    public async Task GetShopWithProductsByIdAsync_ShopWithProducts()
    {
        var context = _contextFakeBuilder.WithShop().WithProduct().Build();
        _sut = new ShopService(context);

        var shop = await _sut.GetShopWithProductsByIdAsync(1);

        Assert.NotNull(shop);
        Assert.Equal(1, shop.Id);
        
        var products = shop.Products.ToList();
        Assert.Equal(2, products.Count);
        Assert.Equal(1, products[0].Id);
        Assert.Equal(3, products[1].Id);
    }
}
