using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Common.Dto;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.ECommerce;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Models.ECommerce;
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
            transactItems.AddRange(postOrderDto.OrderItems.Select(orderItemModel =>
                {
                    int.TryParse(orderItemModel.Amount, out var amount);
                    int.TryParse(orderItemModel.Price, out var price);
                    return new OrderItem
                    {
                        Amount = amount,
                        Description = orderItemModel.Description,
                        ItemId = orderItemModel.OrderItemId,
                        OrderId = order.OrderId,
                        Price = price,
                        Type = nameof(OrderItem)
                    };
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

    public async Task<GetOrderDto> GetByOrderIdAsync(string orderId)
    {
        try
        {
            var gsi1Pk = Order.OrderIdToGsi1Pk(orderId);
            var queryRequest = new QueryRequest
            {
                TableName = _tableName,
                IndexName = "GSI1",
                KeyConditionExpression = "#gsi1pk = :pk",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":pk", new AttributeValue {S = gsi1Pk}}
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#gsi1pk", "GSI1PK"}
                },
                ScanIndexForward = false
            };
            var response = await _client.QueryAsync(queryRequest);
            var order = response.Items.FirstOrDefault();
            if (order == null)
                return new GetOrderDto
                {
                    Order = null,
                    OrderItems = null,
                    Succeeded = false,
                    Message = "Order not found"
                };
            var orderItems = response.Items
                .Where(item =>
                    item["Type"].S == nameof(OrderItem))
                .ToList();
            var orderItemModels = orderItems
                .Select(orderItem => new OrderItemModel
                {
                    OrderId = orderItem["OrderId"].S,
                    Amount = orderItem["Amount"].N,
                    Description = orderItem["Description"].S,
                    OrderItemId = orderItem["ItemId"].S,
                    Price = orderItem["Price"].N
                }).ToList();
            return new GetOrderDto
            {
                Order = new OrderModel
                {
                    OrderId = order["OrderId"].S,
                    Address = new Address
                    {
                        Country = order["Address"].M["Country"].S,
                        PostalCode = order["Address"].M["PostalCode"].S,
                        StreetAddress = order["Address"].M["StreetAddress"].S
                    },
                    CreatedAt = order["CreatedAt"].S,
                    NumberOfItems = order["NumberOfItems"].N,
                    Status = order["Status"].S,
                    TotalAmount = order["TotalAmount"].N,
                    UserName = order["UserName"].S
                },
                OrderItems = orderItemModels,
                Succeeded = true,
                Message = "Success"
            };
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetOrderDto
            {
                Order = null,
                OrderItems = null,
                Message = e.Message,
                Succeeded = false
            };
        }
    }
}
