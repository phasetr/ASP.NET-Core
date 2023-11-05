using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Common.Dto;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.ECommerce;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce.Interfaces;

namespace WebApiDynamodbLocal.Services.ECommerce;

public class OrderService : IOrderService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<OrderService> _logger;
    private readonly string _tableName;

    public OrderService(
        AmazonDynamoDBClient client,
        IConfiguration configuration,
        ILogger<OrderService> logger)
    {
        _client = client;
        _logger = logger;
        _tableName = configuration[AwsSettings.ConfigurationECommerceTable];
    }

    public async Task<ResponseBaseDto> CreateAsync(PostOrderDto postOrderDto)
    {
        var dateTime = DateTime.UtcNow;
        var order = new Order
        {
            Type = nameof(Order),
            UserName = postOrderDto.UserName,
            OrderId = Order.GenerateOrderId(dateTime),
            Address = postOrderDto.Address,
            CreatedAt = dateTime,
            Status = postOrderDto.Status,
            TotalAmount = postOrderDto.TotalAmount,
            NumberOfItems = postOrderDto.NumberOfItems
        };
        try
        {
            var transactItems = new List<TransactWriteItem>
            {
                new()
                {
                    Put = new Put
                    {
                        ConditionExpression = "attribute_not_exists(PK)",
                        Item = order.ToDynamoDbItem(),
                        TableName = _tableName
                    }
                }
            };
            transactItems.AddRange(postOrderDto.OrderItems.Select(orderItemModel => new OrderItem
                {
                    Amount = orderItemModel.Amount,
                    Description = orderItemModel.Description,
                    ItemId = orderItemModel.OrderItemId,
                    OrderId = order.OrderId,
                    Price = orderItemModel.Price,
                    Type = nameof(OrderItem)
                })
                .Select(orderItem =>
                    new TransactWriteItem
                    {
                        Put =
                            new Put
                            {
                                Item = orderItem.ToDynamoDbItem(),
                                TableName = _tableName
                            }
                    }));
            var request = new TransactWriteItemsRequest {TransactItems = transactItems};
            var response = await _client.TransactWriteItemsAsync(request);
            return new ResponseBaseDto
            {
                Succeeded = true,
                Message = response.HttpStatusCode == HttpStatusCode.OK ? "Success" : "Failed"
            };
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseDto
            {
                Succeeded = false,
                Message = e.Message
            };
        }
    }
}
