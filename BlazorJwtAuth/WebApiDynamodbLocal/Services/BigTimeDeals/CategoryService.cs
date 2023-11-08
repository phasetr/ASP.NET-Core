using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Common.Dto;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Category;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Services.BigTimeDeals;

public class CategoryService : ICategoryService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<CategoryService> _logger;
    private readonly string _tableName;

    public CategoryService(
        AmazonDynamoDBClient client,
        IConfiguration configuration,
        ILogger<CategoryService> logger)
    {
        _client = client;
        _logger = logger;
        _tableName = configuration[AwsSettings.ConfigurationBigTimeDealsTable];
    }

    public async Task<ResponseBaseDto> CreateAsync(Category category)
    {
        try
        {
            await _client.PutItemAsync(new PutItemRequest
            {
                TableName = _tableName,
                ConditionExpression = "attribute_not_exists(PK)",
                Item = category.ToDynamoDbItem()
            });
            return new ResponseBaseDto
            {
                Succeeded = true,
                Message = "Category created successfully"
            };
        }
        catch (TransactionCanceledException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            if (e.ErrorCode == "ConditionalCheckFailedException")
                return new ResponseBaseDto
                {
                    Succeeded = false,
                    Message = "Category with this name already exists"
                };
            return new ResponseBaseDto
            {
                Succeeded = false,
                Message = e.Message
            };
        }
    }

    public async Task<GetResponseDto> GetAsync(string name)
    {
        try
        {
            var response = await _client.GetItemAsync(new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Category.NameToPk(name))},
                    {"SK", new AttributeValue(Category.NameToSk(name))}
                }
            });
            if (response.Item == null)
                return new GetResponseDto
                {
                    Succeeded = false,
                    Message = "Category not found"
                };

            return new GetResponseDto
            {
                Succeeded = true,
                Message = "Category found",
                CategoryModel = new CategoryModel
                {
                    Name = response.Item["Name"].S,
                    FeaturedDeals = response.Item["FeaturedDeals"].S,
                    LikeCount = Convert.ToInt32(response.Item["LikeCount"].N),
                    WatchCount = Convert.ToInt32(response.Item["WatchCount"].N)
                }
            };
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetResponseDto
            {
                Succeeded = false,
                Message = e.Message
            };
        }
    }
}
