using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Common.Dto;
using WebApiDynamodbLocal.Constants;
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

    public async Task<bool> DeleteAsync(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Customer>> GetCustomersAsync(int limit = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<GetResponseCustomerDto?> GetByUserNameAsync(string userName)
    {
        try
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Customer.ToPk(userName))},
                    {"SK", new AttributeValue(Customer.ToSk(userName))}
                }
            };
            var response = await _client.GetItemAsync(request);
            // 項目が取れているか確認
            if (response.Item == null || response.Item.Count == 0)
                return new GetResponseCustomerDto
                {
                    CustomerModel = null,
                    Message = "Not Found",
                    Succeeded = false
                };

            var item = response.Item;
            return new GetResponseCustomerDto
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
            return new GetResponseCustomerDto
            {
                CustomerModel = null,
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}
