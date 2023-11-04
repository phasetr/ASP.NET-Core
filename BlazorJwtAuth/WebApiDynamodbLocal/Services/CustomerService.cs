using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Common.Dto;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Services.Interfaces;

namespace WebApiDynamodbLocal.Services;

public class CustomerService : ICustomerService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        AmazonDynamoDBClient client,
        ILogger<CustomerService> logger)
    {
        _client = client;
        _logger = logger;
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
                        Item = customerEmail.ToDynamoDbItem(),
                        TableName = AwsSettings.ECommerceTable
                    }
                },
                new()
                {
                    Put = new Put
                    {
                        Item = customerEmail.ToDynamoDbItem(),
                        TableName = AwsSettings.ECommerceTable
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
        catch (AmazonDynamoDBException e)
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

    public async Task<bool> DeleteAsync(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Customer>> GetCustomersAsync(int limit = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<GetCustomerDto?> GetByUserNameAsync(string userName)
    {
        try
        {
            var pk = new Customer().UserNameToPk(userName);
            var request = new GetItemRequest
            {
                TableName = AwsSettings.ECommerceTable,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(pk)},
                    {"SK", new AttributeValue(pk)}
                }
            };
            var response = await _client.GetItemAsync(request);
            if (response.Item == null)
                return new GetCustomerDto
                {
                    Customer = null,
                    Message = "Not Found",
                    Succeeded = false
                };

            var item = response.Item;
            return new GetCustomerDto
            {
                Customer = new Customer
                {
                    Pk = item["PK"].S,
                    Sk = item["SK"].S,
                    Type = item["Type"].S,
                    UserName = item["UserName"].S,
                    Email = item["Email"].S,
                    Name = item["Name"].S,
                    Addresses = new Dictionary<string, Address>()
                },
                Message = "Success",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetCustomerDto
            {
                Customer = null,
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
