using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;
using WebApiDynamodbLocal.Constants;
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
            Status = "Pending",
            TotalAmount = 300,
            NumberOfItems = 2,
            OrderItemModels = new OrderItemModel[]
            {
                new()
                {
                    OrderItemId = "1",
                    Description = "description1",
                    Price = "100",
                    Amount = "1"
                },
                new()
                {
                    OrderItemId = "2",
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
}
