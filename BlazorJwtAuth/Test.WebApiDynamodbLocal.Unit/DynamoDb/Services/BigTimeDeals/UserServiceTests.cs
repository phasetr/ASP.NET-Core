using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals;

namespace Test.WebApiDynamodbLocal.Unit.DynamoDb.Services.BigTimeDeals;

public class UserServiceTests
{
    private readonly User _user = new()
    {
        UserName = "TestUserName",
        Name = "TestName",
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task CreateAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationBigTimeDealsTable].Returns(tableName);
        var sut = new UserService(client, mockConfiguration, mockLogger);

        // 上記ユーザーはテーブルに存在しない
        var getResult = await sut.GetAsync(_user.UserName);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // ユーザーを作成
        var createResult = await sut.CreateAsync(_user);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("User created successfully", createResult.Message);

        // ユーザー作成を確認
        var getResult2 = await sut.GetAsync(_user.UserName);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.UserModel);
        Assert.Equal(_user.UserName, getResult2.UserModel.UserName);
    }
}
