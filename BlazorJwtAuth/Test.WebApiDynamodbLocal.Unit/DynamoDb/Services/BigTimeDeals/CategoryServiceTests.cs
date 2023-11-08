using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals;

namespace Test.WebApiDynamodbLocal.Unit.DynamoDb.Services.BigTimeDeals;

public class CategoryServiceTests
{
    private readonly Category _category = new()
    {
        Name = "category name",
        FeaturedDeals = "featured deals",
        LikeCount = 1,
        WatchCount = 2
    };

    [Fact]
    public async Task CreateAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CategoryService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationBigTimeDealsTable].Returns(tableName);
        var sut = new CategoryService(client, mockConfiguration, mockLogger);

        // 上記カテゴリはテーブルに存在しない
        var getResult = await sut.GetAsync(_category.Name);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // カテゴリを作成
        var createResult = await sut.CreateAsync(_category);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Category created successfully", createResult.Message);

        // カテゴリ作成を確認
        var getResult2 = await sut.GetAsync(_category.Name);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.CategoryModel);
        Assert.Equal(_category.Name, getResult2.CategoryModel.Name);
        Assert.Equal(_category.FeaturedDeals, getResult2.CategoryModel.FeaturedDeals);
    }

    [Fact]
    public async Task GetLatestDealsAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CategoryService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationBigTimeDealsTable].Returns(tableName);
        var sut = new CategoryService(client, mockConfiguration, mockLogger);

        // 上記カテゴリはテーブルに存在しない
        var getResult = await sut.GetAsync(_category.Name);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // カテゴリを作成
        var createResult = await sut.CreateAsync(_category);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Category created successfully", createResult.Message);

        // ディールを作成
        var mockDealLogger = Substitute.For<ILogger<DealService>>();
        var dealService = new DealService(client, mockConfiguration, mockDealLogger);
        var createdAt1 = new DateTime(2023, 1, 1, 1, 0, 0, DateTimeKind.Utc);
        await dealService.CreateAsync(new Deal
        {
            DealId = Key.GenerateKsuId(createdAt1),
            Title = "title",
            Link = "link1",
            Price = 100,
            Category = _category.Name,
            Brand = "brand",
            CreatedAt = createdAt1
        });
        var createdAt2 = new DateTime(2023, 1, 1, 2, 0, 0, DateTimeKind.Utc);
        await dealService.CreateAsync(new Deal
        {
            DealId = Key.GenerateKsuId(createdAt2),
            Title = "title",
            Link = "link1",
            Price = 100,
            Category = _category.Name,
            Brand = "brand",
            CreatedAt = createdAt2
        });

        // カテゴリの最新ディールを取得
        var getLatestDealsResult = await sut.GetLatestDealsAsync(_category.Name, createdAt1, 2);
        Assert.NotNull(getLatestDealsResult);
        Assert.True(getLatestDealsResult.Succeeded);
        Assert.NotNull(getLatestDealsResult.LatestDeals);
        Assert.Equal(2, getLatestDealsResult.LatestDeals.Count);
    }
}
