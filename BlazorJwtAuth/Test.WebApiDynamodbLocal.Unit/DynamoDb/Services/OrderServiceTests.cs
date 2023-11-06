using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Constants.ECommerce;
using WebApiDynamodbLocal.Dto.ECommerce.Order;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Models.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce;

namespace Test.WebApiDynamodbLocal.Unit.DynamoDb.Services;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<OrderService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new OrderService(client, mockConfiguration, mockLogger);

        var postOrderDto = new PostOrderDto
        {
            UserName = "user",
            Address = new Address
            {
                StreetAddress = "123 Main St",
                PostalCode = "12345",
                Country = "USA"
            },
            TotalAmount = 300,
            NumberOfItems = 2,
            OrderItemModels = new OrderItemModel[]
            {
                new()
                {
                    Description = "description1",
                    Price = "100",
                    Amount = "1"
                },
                new()
                {
                    Description = "description2",
                    Price = "200",
                    Amount = "2"
                }
            }
        };

        // 注文を作成
        var createResult = await sut.CreateAsync(postOrderDto);
        var orderId = createResult.OrderId;
        Assert.NotNull(createResult);
        Assert.NotNull(orderId);
        Assert.True(createResult.Succeeded);
        Assert.Equal("Success", createResult.Message);

        // 注文作成を確認
        var getResult2 = await sut.GetByOrderIdAsync(orderId);
        Assert.NotNull(getResult2);
        Assert.True(getResult2.Succeeded);
        Assert.NotNull(getResult2.OrderModel);
        Assert.NotNull(getResult2.OrderItemModels);
        Assert.Equal(2, getResult2.OrderItemModels.Count());
        Assert.Equal(orderId, getResult2.OrderModel.OrderId);
    }

    [Fact]
    public async Task GetByUserNameAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<OrderService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new OrderService(client, mockConfiguration, mockLogger);

        var dto1 = new PostOrderDto
        {
            UserName = "user",
            Address = new Address
            {
                StreetAddress = "123 Main St",
                PostalCode = "12345",
                Country = "USA"
            },
            TotalAmount = 300,
            NumberOfItems = 2,
            OrderItemModels = new OrderItemModel[]
            {
                new()
                {
                    Description = "description1",
                    Price = "100",
                    Amount = "1"
                },
                new()
                {
                    Description = "description2",
                    Price = "200",
                    Amount = "2"
                }
            }
        };
        var dto2 = new PostOrderDto
        {
            UserName = "user",
            Address = new Address
            {
                StreetAddress = "123 Main St",
                PostalCode = "12345",
                Country = "USA"
            },
            TotalAmount = 700,
            NumberOfItems = 7,
            OrderItemModels = new OrderItemModel[]
            {
                new()
                {
                    Description = "description3",
                    Price = "300",
                    Amount = "3"
                },
                new()
                {
                    Description = "description4",
                    Price = "400",
                    Amount = "4"
                }
            }
        };

        // 注文を作成
        await sut.CreateAsync(dto1);
        await sut.CreateAsync(dto2);

        // ユーザー名で注文を取得
        var getResult = await sut.GetByUserNameAsync(dto1.UserName);
        Assert.NotNull(getResult);
        Assert.Equal("user", getResult.UserName);
        Assert.True(getResult.Succeeded);
        Assert.NotNull(getResult.OrderModels);
        Assert.Equal(2, getResult.OrderModels.Count);
        Assert.Equal("700", getResult.OrderModels[0].TotalAmount);
        Assert.Equal("300", getResult.OrderModels[1].TotalAmount);
    }

    [Fact]
    public async Task GetByUserNameAsync_CustomerNotExist()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<OrderService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new OrderService(client, mockConfiguration, mockLogger);

        var dto1 = new PostOrderDto
        {
            UserName = "user",
            Address = new Address
            {
                StreetAddress = "123 Main St",
                PostalCode = "12345",
                Country = "USA"
            },
            TotalAmount = 300,
            NumberOfItems = 2,
            OrderItemModels = new OrderItemModel[]
            {
                new()
                {
                    Description = "description1",
                    Price = "100",
                    Amount = "1"
                },
                new()
                {
                    Description = "description2",
                    Price = "200",
                    Amount = "2"
                }
            }
        };

        // 注文を作成
        await sut.CreateAsync(dto1);

        var getResult = await sut.GetByUserNameAsync("noUser");
        Assert.NotNull(getResult);
        Assert.False(getResult.Succeeded);
        Assert.Null(getResult.OrderModels);
        Assert.Equal("Customer does not exist", getResult.Message);
    }

    [Fact]
    public async Task PutStatusAsync_Succeeded()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<OrderService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new OrderService(client, mockConfiguration, mockLogger);

        var dto1 = new PostOrderDto
        {
            UserName = "user",
            Address = new Address
            {
                StreetAddress = "123 Main St",
                PostalCode = "12345",
                Country = "USA"
            },
            TotalAmount = 300,
            NumberOfItems = 2,
            OrderItemModels = new OrderItemModel[]
            {
                new()
                {
                    Description = "description1",
                    Price = "100",
                    Amount = "1"
                },
                new()
                {
                    Description = "description2",
                    Price = "200",
                    Amount = "2"
                }
            }
        };

        // 注文を作成
        var createResult = await sut.CreateAsync(dto1);
        var orderId = createResult.OrderId;
        Assert.NotNull(orderId);

        // 注文ステータスを更新
        var putResult = await sut.PutStatusAsync(dto1.UserName, orderId, nameof(OrderStatus.Delivered));
        Assert.NotNull(putResult);
        Assert.True(putResult.Succeeded);
        Assert.Equal("Success", putResult.Message);

        // 注文ステータスを確認
        var getResult = await sut.GetByOrderIdAsync(orderId);
        Assert.NotNull(getResult);
        Assert.True(getResult.Succeeded);
        Assert.NotNull(getResult.OrderModel);
        Assert.Equal(nameof(OrderStatus.Delivered), getResult.OrderModel.Status);
    }

    [Fact]
    public async Task PutStatusAsync_OrderNotExist()
    {
        var builder = new AmazonDynamoDbClientFakeBuilder().Build();
        var client = builder.Client;
        var tableName = builder.TableName;

        var mockLogger = Substitute.For<ILogger<OrderService>>();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration[AwsSettings.ConfigurationECommerceTable].Returns(tableName);
        var sut = new OrderService(client, mockConfiguration, mockLogger);

        var dto1 = new PostOrderDto
        {
            UserName = "user",
            Address = new Address
            {
                StreetAddress = "123 Main St",
                PostalCode = "12345",
                Country = "USA"
            },
            TotalAmount = 300,
            NumberOfItems = 2,
            OrderItemModels = new OrderItemModel[]
            {
                new()
                {
                    Description = "description1",
                    Price = "100",
                    Amount = "1"
                },
                new()
                {
                    Description = "description2",
                    Price = "200",
                    Amount = "2"
                }
            }
        };

        // 注文を作成
        var response = await sut.CreateAsync(dto1);
        var orderId = response.OrderId;
        Assert.NotNull(orderId);

        // 注文ステータスを更新
        var putResult = await sut.PutStatusAsync("noUser", orderId, nameof(OrderStatus.Delivered));
        Assert.NotNull(putResult);
        Assert.False(putResult.Succeeded);
        Assert.Equal("Order does not exist", putResult.Message);
    }
}
