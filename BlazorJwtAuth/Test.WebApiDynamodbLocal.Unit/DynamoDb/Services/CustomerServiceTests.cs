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
    private readonly Customer _customer = new()
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

        // 上記ユーザーはテーブルに存在しない
        var getResult = await sut.GetByUserNameAsync(_customer.UserName);
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);

        // ユーザーを作成
        var createResult = await sut.CreateAsync(_customer);
        Assert.NotNull(createResult);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Success", createResult.Message);

        // ユーザー作成を確認
        var getResult2 = await sut.GetByUserNameAsync(_customer.UserName);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.CustomerModel);
        Assert.Equal(_customer.UserName, getResult2.CustomerModel.UserName);
    }

    [Fact]
    public async Task CreateAsync_DuplicatedUserName()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CustomerService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new CustomerService(client, mockConfiguration, mockLogger);

        // UserNameが一致するユーザーを作成
        var customer = new Customer
        {
            Addresses = _customer.Addresses,
            Email = "converted@phasetr.com",
            Name = _customer.Name,
            Type = _customer.Type,
            UserName = _customer.UserName
        };

        // ユーザーを登録
        _ = await sut.CreateAsync(_customer);
        var createResult2 = await sut.CreateAsync(customer);
        Assert.NotNull(createResult2);
        Assert.False(createResult2.Succeeded);
        Assert.Equal("Customer with this username already exists", createResult2.Message);
    }

    [Fact]
    public async Task CreateAsync_DuplicatedEmail()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CustomerService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new CustomerService(client, mockConfiguration, mockLogger);

        // Emailが一致するユーザーを作成
        var customer = new Customer
        {
            Addresses = _customer.Addresses,
            Email = _customer.Email,
            Name = "converted",
            Type = _customer.Type,
            UserName = "converted"
        };

        // ユーザーを登録
        _ = await sut.CreateAsync(_customer);
        var createResult2 = await sut.CreateAsync(customer);
        Assert.NotNull(createResult2);
        Assert.False(createResult2.Succeeded);
        Assert.Equal("Customer with this email already exists", createResult2.Message);
    }
}
