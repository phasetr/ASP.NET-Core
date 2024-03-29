using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.SessionStore;
using WebApiDynamodbLocal.Entities.SessionStore;
using WebApiDynamodbLocal.Services.SessionStore.interfaces;
using ResponseBaseDto = WebApiDynamodbLocal.Dto.ResponseBaseDto;

namespace WebApiDynamodbLocal.Services.SessionStore;

public class SessionService : ISessionService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<SessionService> _logger;
    private readonly string _tableName;

    public SessionService(
        AmazonDynamoDBClient client,
        IConfiguration configuration,
        ILogger<SessionService> logger)
    {
        var tableName = configuration[AwsSettings.ConfigurationSessionStoreTable];
        ArgumentNullException.ThrowIfNull(tableName);
        _client = client;
        _logger = logger;
        _tableName = tableName;
    }

    public async Task<ResponseBaseWithKeyDto> CreateAsync(PostDto dto)
    {
        try
        {
            var sessionId = Guid.NewGuid().ToString();
            var createdAt = DateTime.UtcNow;
            var expiredAt = createdAt.AddDays(7);
            var ttl = (int) (expiredAt - createdAt).TotalSeconds;
            await _client.PutItemAsync(new PutItemRequest
            {
                TableName = _tableName,
                ConditionExpression = "attribute_not_exists(PK)",
                Item = new Session
                {
                    SessionId = sessionId,
                    UserName = dto.UserName,
                    CreatedAt = createdAt,
                    ExpiredAt = expiredAt,
                    Ttl = ttl
                }.ToDynamoDbItem()
            });
            return new ResponseBaseWithKeyDto
            {
                Key = sessionId,
                Message = "Session created successfully",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseWithKeyDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<GetResponseDto> GetAsync(string sessionId)
    {
        try
        {
            var response = await _client.GetItemAsync(new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Key.SessionPk(sessionId))},
                    {"SK", new AttributeValue(Key.SessionSk(sessionId))}
                }
            });
            if (response.Item == null || response.Item.Count == 0)
                return new GetResponseDto
                {
                    Message = "Session not found",
                    Succeeded = false
                };

            return new GetResponseDto
            {
                Message = "Session found",
                Succeeded = true,
                UserName = response.Item["UserName"].S
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

    public async Task<ResponseBaseDto> DeleteByUserNameAsync(string userName)
    {
        try
        {
            var gsi1Pk = Key.SessionGsi1Pk(userName);
            var response = await _client.QueryAsync(new QueryRequest
            {
                TableName = _tableName,
                IndexName = "GSI1",
                KeyConditionExpression = "#gsi1pk = :pk",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#gsi1pk", "GSI1PK"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":pk", new AttributeValue {S = gsi1Pk}}
                }
            });
            if (response.Items == null || response.Items.Count == 0)
                return new ResponseBaseDto
                {
                    Message = "Session not found",
                    Succeeded = false
                };

            var deleteItems = response.Items.Select(item => new WriteRequest
            {
                DeleteRequest = new DeleteRequest
                    {Key = new Dictionary<string, AttributeValue> {{"PK", item["PK"]}, {"SK", item["SK"]}}}
            }).ToList();
            var request = new BatchWriteItemRequest
                {RequestItems = new Dictionary<string, List<WriteRequest>> {{_tableName, deleteItems}}};
            await _client.BatchWriteItemAsync(request);

            return new ResponseBaseDto
            {
                Message = "Session deleted successfully",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }
}
