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
        Type = nameof(Category),
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
}
