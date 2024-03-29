using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Constants.ECommerce;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Order;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Models.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce.Interfaces;
using ResponseBaseDto = WebApiDynamodbLocal.Dto.ResponseBaseDto;

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

    public async Task<ResponseBaseWithKeyDto> CreateAsync(PostOrderDto postOrderDto)
    {
        var dateTime = DateTime.UtcNow;
        var orderId = Key.GenerateKsuId(dateTime);
        var order = new Order
        {
            UserName = postOrderDto.UserName,
            OrderId = orderId,
            Address = postOrderDto.Address,
            CreatedAt = dateTime,
            Status = nameof(OrderStatus.Placed),
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
            transactItems.AddRange(postOrderDto.OrderItemModels
                .Select((orderItemModel, i) =>
                {
                    int.TryParse(orderItemModel.Amount, out var amount);
                    int.TryParse(orderItemModel.Price, out var price);
                    return new OrderItem
                    {
                        Amount = amount,
                        Description = orderItemModel.Description,
                        ItemId = i + 1,
                        OrderId = orderId,
                        Price = price
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
            return new ResponseBaseWithKeyDto
            {
                Key = orderId,
                Succeeded = true,
                Message = response.HttpStatusCode == HttpStatusCode.OK ? "Success" : "Failed"
            };
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseWithKeyDto
            {
                Succeeded = false,
                Message = e.Message
            };
        }
    }

    public async Task<GetOrderResponseDto> GetByOrderIdAsync(string orderId)
    {
        try
        {
            var gsi1Pk = Key.OrderGsi1Pk(orderId);
            var queryRequest = new QueryRequest
            {
                TableName = _tableName,
                IndexName = "GSI1",
                KeyConditionExpression = "#gsi1pk = :pk",
                ExpressionAttributeNames = new Dictionary<string, string>
                    {{"#gsi1pk", "GSI1PK"}},
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {{":pk", new AttributeValue {S = gsi1Pk}}},
                ScanIndexForward = false
            };
            var response = await _client.QueryAsync(queryRequest);
            var order = response.Items.FirstOrDefault();
            if (order == null)
                return new GetOrderResponseDto
                {
                    OrderModel = null,
                    OrderItemModels = null,
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
                    Amount = orderItem["Amount"].N,
                    Description = orderItem["Description"].S,
                    Price = orderItem["Price"].N
                }).ToList();
            return new GetOrderResponseDto
            {
                OrderModel = new OrderModel
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
                OrderItemModels = orderItemModels,
                Succeeded = true,
                Message = "Success"
            };
        }
        catch (TransactionCanceledException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            if (e.ErrorCode != "TransactionCanceledException")
                return new GetOrderResponseDto
                {
                    OrderModel = null,
                    OrderItemModels = null,
                    Message = e.Message,
                    Succeeded = false
                };
            var message = e.Message;
            if (e.CancellationReasons[0].Code == "ConditionalCheckFailed")
                message = "OrderId already exists for this customer";
            return new GetOrderResponseDto
            {
                OrderModel = null,
                OrderItemModels = null,
                Message = message,
                Succeeded = false
            };
        }
    }

    public async Task<GetOrdersResponseDto> GetByUserNameAsync(string userName)
    {
        try
        {
            var queryRequest = new QueryRequest
            {
                TableName = _tableName,
                KeyConditionExpression = "#pk = :pk",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#pk", "PK"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":pk", new AttributeValue {S = Key.CustomerPk(userName)}}
                },
                ScanIndexForward = false
            };
            var response = await _client.QueryAsync(queryRequest);
            // 該当顧客が存在しない
            if (response.Items.Count == 0)
                return new GetOrdersResponseDto
                {
                    Message = "Customer does not exist",
                    Succeeded = false
                };
            var orders = response.Items
                .Where(item =>
                    item["Type"].S == nameof(Order))
                .ToList();
            var orderModels = orders
                .Select(order => new OrderModel
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
                }).ToList();
            return new GetOrdersResponseDto
            {
                UserName = userName,
                OrderModels = orderModels,
                Succeeded = true,
                Message = "Success"
            };
        }
        catch (TransactionCanceledException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetOrdersResponseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<ResponseBaseDto> PutStatusAsync(string userName, string orderId, string status)
    {
        try
        {
            var updateRequest = new UpdateItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue {S = Key.OrderPk(userName)}},
                    {"SK", new AttributeValue {S = Key.OrderSk(orderId)}}
                },
                ConditionExpression = "attribute_exists(PK) AND attribute_exists(SK)",
                UpdateExpression = "SET #status = :status",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#status", "Status"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":status", new AttributeValue {S = status}}
                }
            };
            var response = await _client.UpdateItemAsync(updateRequest);
            return new ResponseBaseDto
            {
                Succeeded = response.HttpStatusCode == HttpStatusCode.OK,
                Message = response.HttpStatusCode == HttpStatusCode.OK ? "Success" : "Failed"
            };
        }
        catch (ConditionalCheckFailedException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            if (e.ErrorCode == "ConditionalCheckFailedException")
                return new ResponseBaseDto
                {
                    Message = "Order does not exist",
                    Succeeded = false
                };
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }
}
