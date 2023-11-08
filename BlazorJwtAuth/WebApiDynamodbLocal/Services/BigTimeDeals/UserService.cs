using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Common.Dto;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.BigTimeDeals.User;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Services.BigTimeDeals;

public class UserService : IUserService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<UserService> _logger;
    private readonly string _tableName;

    public UserService(
        AmazonDynamoDBClient client,
        IConfiguration configuration,
        ILogger<UserService> logger)
    {
        _client = client;
        _logger = logger;
        _tableName = configuration[AwsSettings.ConfigurationBigTimeDealsTable];
    }

    public async Task<ResponseBaseDto> CreateAsync(User user)
    {
        try
        {
            await _client.PutItemAsync(new PutItemRequest
            {
                TableName = _tableName,
                ConditionExpression = "attribute_not_exists(PK)",
                Item = user.ToDynamoDbItem()
            });
            return new ResponseBaseDto
            {
                Message = "User created successfully",
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
                    Message = "User with this username already exists",
                    Succeeded = false
                };
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<GetResponseDto> GetAsync(string userName)
    {
        try
        {
            var response = await _client.GetItemAsync(new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(User.ToPk(userName))},
                    {"SK", new AttributeValue(User.ToPk(userName))}
                }
            });
            if (response.Item == null || response.Item.Count == 0)
                return new GetResponseDto
                {
                    Message = "User not found",
                    Succeeded = false
                };
            return new GetResponseDto
            {
                UserModel = new UserModel
                {
                    UserName = response.Item["UserName"].S,
                    Name = response.Item["Name"].S
                },
                Message = "User found successfully",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetResponseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }
}
