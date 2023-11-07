using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using KsuidDotNet;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Deal;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Services.BigTimeDeals;

public class DealService : IDealService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<DealService> _logger;
    private readonly string _tableName;

    public DealService(
        AmazonDynamoDBClient client,
        IConfiguration configuration,
        ILogger<DealService> logger)
    {
        _client = client;
        _logger = logger;
        _tableName = configuration[AwsSettings.ConfigurationBigTimeDealsTable];
    }

    public async Task<PostResponseDto> CreateAsync(Deal deal)
    {
        try
        {
            var createdAt = DateTime.UtcNow;
            var dealId = Ksuid.NewKsuid(createdAt);
            deal.CreatedAt = createdAt;
            deal.DealId = dealId;
            await _client.PutItemAsync(new PutItemRequest
            {
                TableName = _tableName,
                ConditionExpression = "attribute_not_exists(PK)",
                Item = deal.ToDynamoDbItem()
            });
            return new PostResponseDto
            {
                DealId = dealId,
                Message = "Deal created successfully",
                Succeeded = true
            };
        }
        catch (TransactionCanceledException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            if (e.ErrorCode == "ConditionalCheckFailedException")
                return new PostResponseDto
                {
                    Message = "Deal with this ID already exists",
                    Succeeded = false
                };
            return new PostResponseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<GetResponseDto> GetAsync(string dealId)
    {
        try
        {
            var response = await _client.GetItemAsync(new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Deal.DealIdToPk(dealId))},
                    {"SK", new AttributeValue(Deal.DealIdToSk(dealId))}
                }
            });
            if (response.Item == null || response.Item.Count == 0)
                return new GetResponseDto
                {
                    Message = "Deal not found",
                    Succeeded = false
                };
            return new GetResponseDto
            {
                DealModel = new DealModel
                {
                    Brand = response.Item["Brand"].S,
                    Category = response.Item["Category"].S,
                    CreatedAt = DateTime.Parse(response.Item["CreatedAt"].S),
                    DealId = response.Item["DealId"].S,
                    Link = response.Item["Link"].S,
                    Price = decimal.Parse(response.Item["Price"].N),
                    Title = response.Item["Title"].S
                },
                Message = "Deal found",
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
