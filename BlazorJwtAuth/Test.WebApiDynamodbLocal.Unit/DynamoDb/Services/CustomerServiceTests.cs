using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce;

namespace Test.WebApiDynamodbLocal.Unit.DynamoDb.Services;

public class CustomerServiceTests
{
    [Fact]
    public async Task CreateAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CustomerService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new CustomerService(client, mockConfiguration, mockLogger);

        var customer = new Customer
        {
            Type = nameof(Customer),
            UserName = "user",
            Email = "user@phasetr.com",
            Name = "user",
            Addresses = new Dictionary<string, Address>
            {
                {
                    "Home", new Address
                    {
                        StreetAddress = "123 Main St",
                        PostalCode = "12345",
                        Country = "USA"
                    }
                }
            }
        };

        // 上記ユーザーはテーブルに存在しない
        var getResult = await sut.GetByUserNameAsync(customer.UserName);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // ユーザーを作成
        var createResult = await sut.CreateAsync(customer);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Success", createResult.Message);

        // ユーザー作成を確認
        var getResult2 = await sut.GetByUserNameAsync(customer.UserName);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
    }
}
