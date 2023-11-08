using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.SessionStore;
using WebApiDynamodbLocal.Entities.SessionStore;
using WebApiDynamodbLocal.Services.SessionStore;

namespace Test.WebApiDynamodbLocal.Unit.DynamoDb.Services.SessionStore;

public class SessionServiceTests
{
    private readonly Session _session = new()
    {
        CreatedAt = new DateTime(2023, 1, 1, 1, 0, 0, DateTimeKind.Utc),
        ExpiredAt = new DateTime(2023, 1, 8, 1, 0, 0, DateTimeKind.Utc),
        SessionId = Guid.NewGuid().ToString(),
        Ttl = (int) (new DateTime(2023, 1, 8, 1, 0, 0, DateTimeKind.Utc) -
                     new DateTime(2023, 1, 1, 1, 0, 0, DateTimeKind.Utc)).TotalSeconds,
        Type = nameof(Session),
        UserName = "user"
    };

    [Fact]
    public async Task CreateAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<SessionService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationSessionStoreTable].Returns(tableName);
        var sut = new SessionService(client, mockConfiguration, mockLogger);

        // 上記セッションはテーブルに存在しない
        var getResult = await sut.GetAsync(_session.SessionId);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // セッションを作成
        var dto = new PostDto {UserName = _session.UserName};
        var createResult = await sut.CreateAsync(dto);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Session created successfully", createResult.Message);
        Assert.NotNull(createResult.Key);

        // ユーザー作成を確認
        var getResult2 = await sut.GetAsync(createResult.Key);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.UserName);
        Assert.Equal("user", getResult2.UserName);
    }

    [Fact]
    public async Task DeleteByUserNameAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<SessionService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationSessionStoreTable].Returns(tableName);
        var sut = new SessionService(client, mockConfiguration, mockLogger);

        // 上記セッションはテーブルに存在しない
        var getResult = await sut.GetAsync(_session.SessionId);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // セッションを作成
        var dto = new PostDto {UserName = _session.UserName};
        var createResult = await sut.CreateAsync(dto);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Session created successfully", createResult.Message);
        Assert.NotNull(createResult.Key);

        // ユーザー作成を確認
        var getResult2 = await sut.GetAsync(createResult.Key);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.UserName);
        Assert.Equal("user", getResult2.UserName);

        // ユーザー削除を確認
        var deleteResult = await sut.DeleteByUserNameAsync(_session.UserName);
        Assert.NotNull(deleteResult);
        Assert.True(deleteResult.Succeeded);
        Assert.Equal("Session deleted successfully", deleteResult.Message);

        // セッションを削除したことを確認
        var getResult3 = await sut.GetAsync(createResult.Key);
        Assert.NotNull(getResult3);
        Assert.False(getResult3.Succeeded);
    }
}
