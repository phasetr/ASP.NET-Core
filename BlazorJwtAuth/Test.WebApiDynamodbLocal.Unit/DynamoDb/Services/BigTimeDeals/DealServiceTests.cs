using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals;

namespace Test.WebApiDynamodbLocal.Unit.DynamoDb.Services.BigTimeDeals;

public class DealServiceTests
{
    private readonly Deal _deal = new()
    {
        Type = nameof(Deal),
        Brand = "brand",
        Category = "category",
        CreatedAt = new DateTime(2023, 1, 1, 1, 1, 1, DateTimeKind.Utc),
        Link = "https://example.com",
        Price = 100m,
        Title = "title"
    };

    [Fact]
    public async Task CreateAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<DealService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationBigTimeDealsTable].Returns(tableName);
        var sut = new DealService(client, mockConfiguration, mockLogger);

        // ディールを作成
        var createResult = await sut.CreateAsync(_deal);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Deal created successfully", createResult.Message);
        var dealId = createResult.DealId;
        Assert.NotNull(dealId);

        // ディール作成を確認
        var getResult2 = await sut.GetAsync(dealId);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.DealModel);
        Assert.Equal(_deal.DealId, getResult2.DealModel.DealId);
    }
}
