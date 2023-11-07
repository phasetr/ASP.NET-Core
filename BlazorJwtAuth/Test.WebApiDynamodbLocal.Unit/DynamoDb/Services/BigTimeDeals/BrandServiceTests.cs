using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals;

namespace Test.WebApiDynamodbLocal.Unit.DynamoDb.Services.BigTimeDeals;

public class BrandServiceTests
{
    private readonly Brand _brand = new()
    {
        Type = nameof(Brand),
        Name = "brand",
        LogoUrl = "https://example.com/logo.png",
        LikeCount = 1,
        WatchCount = 2
    };

    [Fact]
    public async Task CreateAsync_Success()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<BrandService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationBigTimeDealsTable].Returns(tableName);
        var sut = new BrandService(client, mockConfiguration, mockLogger);

        // 上記ブランドはテーブルに存在しない
        var getResult = await sut.GetAsync(_brand.Name);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // ブランドを作成
        var createResult = await sut.CreateAsync(_brand);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Brand created successfully", createResult.Message);

        // ブランド作成を確認
        var getResult2 = await sut.GetAsync(_brand.Name);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.BrandModel);
        Assert.Equal(_brand.Name, getResult2.BrandModel.Name);
    }
}
