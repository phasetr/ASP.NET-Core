using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Customer;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Models.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce.Interfaces;

namespace WebApiDynamodbLocal.Services.ECommerce;

public class CustomerService : ICustomerService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<CustomerService> _logger;
    private readonly string _tableName;

    public CustomerService(
        AmazonDynamoDBClient client,
        IConfiguration configuration,
        ILogger<CustomerService> logger)
    {
        _client = client;
        _logger = logger;
        _tableName = configuration[AwsSettings.ConfigurationECommerceTable];
    }

    public async Task<ResponseBaseDto> CreateAsync(Customer customer)
    {
        var customerEmail = new CustomerEmail
        {
            Email = customer.Email,
            UserName = customer.UserName
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
                        Item = customer.ToDynamoDbItem(),
                        TableName = _tableName
                    }
                },
                new()
                {
                    Put = new Put
                    {
                        ConditionExpression = "attribute_not_exists(PK)",
                        Item = customerEmail.ToDynamoDbItem(),
                        TableName = _tableName
                    }
                }
            };
            var request = new TransactWriteItemsRequest {TransactItems = transactItems};
            var response = await _client.TransactWriteItemsAsync(request);
            return new ResponseBaseDto
            {
                Succeeded = true,
                Message = response.HttpStatusCode == HttpStatusCode.OK ? "Success" : "Failed"
            };
        }
        catch (TransactionCanceledException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            if (e.ErrorCode != "TransactionCanceledException")
                return new ResponseBaseDto
                {
                    Succeeded = false,
                    Message = e.Message
                };
            // 詳細なメッセージ指定
            var message = string.Empty;
            if (e.CancellationReasons[0].Code == "ConditionalCheckFailed")
                message = "Customer with this username already exists";
            else if (e.CancellationReasons[1].Code == "ConditionalCheckFailed")
                message = "Customer with this email already exists";
            return new ResponseBaseDto
            {
                Succeeded = false,
                Message = message
            };
        }
    }

    public async Task<ResponseBaseDto> DeleteAddressAsync(string userName, string addressName)
    {
        try
        {
            var response = await _client.UpdateItemAsync(new UpdateItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Key.CustomerPk(userName))},
                    {"SK", new AttributeValue(Key.CustomerSk(userName))}
                },
                ConditionExpression = "attribute_exists(PK)",
                UpdateExpression = "REMOVE #prop.#name",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#prop", "Addresses"},
                    {"#name", addressName}
                }
            });
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return new ResponseBaseDto
                {
                    Message = "Failed",
                    Succeeded = false
                };
            return new ResponseBaseDto
            {
                Message = "Success",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            if (e.ErrorCode == "ConditionalCheckFailedException")
                return new ResponseBaseDto
                {
                    Message = "Customer does not exist",
                    Succeeded = false
                };
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<GetCustomerResponseDto?> GetByUserNameAsync(string userName)
    {
        try
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Key.CustomerPk(userName))},
                    {"SK", new AttributeValue(Key.CustomerSk(userName))}
                }
            };
            var response = await _client.GetItemAsync(request);
            // 項目が取れているか確認
            if (response.Item == null || response.Item.Count == 0)
                return new GetCustomerResponseDto
                {
                    CustomerModel = null,
                    Message = "Not Found",
                    Succeeded = false
                };

            var item = response.Item;
            return new GetCustomerResponseDto
            {
                CustomerModel = new CustomerModel
                {
                    UserName = item["UserName"].S,
                    Email = item["Email"].S,
                    Name = item["Name"].S,
                    Addresses = item["Addresses"].M.ToDictionary(
                        x => x.Key,
                        x => new Address
                        {
                            StreetAddress = x.Value.M["StreetAddress"].S,
                            PostalCode = x.Value.M["PostalCode"].S,
                            Country = x.Value.M["Country"].S
                        })
                },
                Message = "Success",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetCustomerResponseDto
            {
                CustomerModel = null,
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<ResponseBaseDto> PutAddressAsync(PutAddressDto putAddressDto)
    {
        try
        {
            var response = await _client.UpdateItemAsync(new UpdateItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Key.CustomerPk(putAddressDto.UserName))},
                    {"SK", new AttributeValue(Key.CustomerSk(putAddressDto.UserName))}
                },
                ConditionExpression = "attribute_exists(PK)",
                UpdateExpression = "SET #prop.#name = :address",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#prop", "Addresses"},
                    {"#name", putAddressDto.AddressName}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":address", new AttributeValue {M = putAddressDto.Address.ToDynamoDbItem()}}
                }
            });
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return new ResponseBaseDto
                {
                    Message = "Failed",
                    Succeeded = false
                };
            return new ResponseBaseDto
            {
                Message = "Success",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);

            if (e.ErrorCode == "ConditionalCheckFailedException")
                return new ResponseBaseDto
                {
                    Message = "Customer does not exist",
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
