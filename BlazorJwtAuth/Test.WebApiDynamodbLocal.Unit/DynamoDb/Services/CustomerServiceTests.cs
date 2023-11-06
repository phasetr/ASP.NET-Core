using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.ECommerce.Customer;
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
            {"Home", new Address {StreetAddress = "123 Main St", PostalCode = "12345", Country = "USA"}},
            {"Work", new Address {StreetAddress = "456 Main St", PostalCode = "23456", Country = "USA"}},
            {"Other", new Address {StreetAddress = "789 Main St", PostalCode = "34567", Country = "USA"}}
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

    [Fact]
    public async Task DeleteAddressAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CustomerService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new CustomerService(client, mockConfiguration, mockLogger);

        // ユーザーを登録
        _ = await sut.CreateAsync(_customer);

        // ユーザーの住所を削除
        var deleteResult = await sut.DeleteAddressAsync(_customer.UserName, "Home");
        Assert.NotNull(deleteResult);
        Assert.True(deleteResult.Succeeded);
        Assert.Equal("Success", deleteResult.Message);

        // ユーザーの住所を削除したことを確認
        var getResult = await sut.GetByUserNameAsync(_customer.UserName);
        Assert.NotNull(getResult);
        Assert.True(getResult.Succeeded);
        Assert.NotNull(getResult.CustomerModel);
        Assert.False(getResult.CustomerModel.Addresses.ContainsKey("Home"));
        Assert.Equal(2, getResult.CustomerModel.Addresses.Count);
        var keys = getResult.CustomerModel.Addresses.Keys.ToArray();
        Assert.Equal("Work", keys[0]);
        Assert.Equal("Other", keys[1]);
    }

    [Fact]
    public async Task DeleteAddressAsync_CustomerNotExist()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CustomerService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new CustomerService(client, mockConfiguration, mockLogger);

        // ユーザーを削除
        var deleteResult = await sut.DeleteAddressAsync(_customer.UserName, "Home");
        Assert.NotNull(deleteResult);
        Assert.False(deleteResult.Succeeded);
        Assert.Equal("Customer does not exist", deleteResult.Message);
    }

    [Fact]
    public async Task PutAddressAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CustomerService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new CustomerService(client, mockConfiguration, mockLogger);

        // ユーザーを登録
        _ = await sut.CreateAsync(_customer);

        // ユーザーの住所を更新
        var putResult = await sut.PutAddressAsync(new PutAddressDto
        {
            UserName = _customer.UserName,
            AddressName = "Home",
            Address = new Address
            {
                StreetAddress = "converted 123 Main St",
                PostalCode = "converted 12345",
                Country = "converted USA"
            }
        });
        Assert.NotNull(putResult);
        Assert.True(putResult.Succeeded);
        Assert.Equal("Success", putResult.Message);

        // ユーザーの住所を更新したことを確認
        var getResult = await sut.GetByUserNameAsync(_customer.UserName);
        Assert.NotNull(getResult);
        Assert.True(getResult.Succeeded);
        Assert.NotNull(getResult.CustomerModel);
        Assert.Equal("converted 123 Main St", getResult.CustomerModel.Addresses["Home"].StreetAddress);
        Assert.Equal("converted 12345", getResult.CustomerModel.Addresses["Home"].PostalCode);
        Assert.Equal("converted USA", getResult.CustomerModel.Addresses["Home"].Country);
    }

    [Fact]
    public async Task PutAddressAsync_CustomerNotExist()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<CustomerService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new CustomerService(client, mockConfiguration, mockLogger);

        // ユーザーの住所を更新
        var putResult = await sut.PutAddressAsync(new PutAddressDto
        {
            UserName = _customer.UserName,
            AddressName = "Home",
            Address = new Address
            {
                StreetAddress = "converted 123 Main St",
                PostalCode = "converted 12345",
                Country = "converted USA"
            }
        });
        Assert.NotNull(putResult);
        Assert.False(putResult.Succeeded);
        Assert.Equal("Customer does not exist", putResult.Message);
    }
}
