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

    public async Task<GetCategoryAndLatestDealsResponseDto> GetCategoryAndLatestDealsAsync(string name)
    {
        try
        {
            var category = await GetAsync(name);
            if (!category.Succeeded)
                return new GetCategoryAndLatestDealsResponseDto
                {
                    Message = category.Message,
                    Errors = category.Errors,
                    Succeeded = false
                };
            var latestDeals = await GetLatestDealsAsync(name, DateTime.UtcNow);
            if (!latestDeals.Succeeded)
                return new GetCategoryAndLatestDealsResponseDto
                {
                    Message = latestDeals.Message,
                    Errors = latestDeals.Errors,
                    Succeeded = false
                };
            return new GetCategoryAndLatestDealsResponseDto
            {
                CategoryModel = category.CategoryModel,
                LatestDealModels = latestDeals.LatestDeals,
                Message = "Category and latest deals found",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetCategoryAndLatestDealsResponseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<GetLatestDealsResponseDto> GetLatestDealsAsync(string name,
        DateTime createdAt, int limit = 25, int count = 0)
    {
        try
        {
            var request = new QueryRequest
            {
                TableName = _tableName,
                IndexName = "GSI3",
                KeyConditionExpression = "#gsi3pk = :gsi3pk",
                ExpressionAttributeNames = new Dictionary<string, string>
                    {{"#gsi3pk", "GSI3PK"}},
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {{":gsi3pk", new AttributeValue(Category.ToGsi3Pk(name, createdAt))}},
                Limit = limit,
                ScanIndexForward = false
            };
            var response = await _client.QueryAsync(request);
            if (response.Items == null || response.Items.Count == 0)
                return new GetLatestDealsResponseDto
                {
                    Message = "Category not found",
                    Succeeded = false
                };
            var latestDeals = response.Items.Select(item => new DealModel
            {
                Brand = item["Brand"].S,
                Category = item["Category"].S,
                DealId = item["DealId"].S,
                Link = item["Link"].S,
                Price = decimal.Parse(item["Price"].N),
                Title = item["Title"].S
            }).ToList();
            if (latestDeals.Count >= limit || count >= 5)
                return new GetLatestDealsResponseDto
                {
                    LatestDeals = latestDeals,
                    Message = "Deals found",
                    Succeeded = true
                };
            var latestDeals2 = await GetLatestDealsAsync(name,
                createdAt.AddDays(-1),
                limit - latestDeals.Count,
                count + 1);
            latestDeals.AddRange(latestDeals2.LatestDeals);
            return new GetLatestDealsResponseDto
            {
                LatestDeals = latestDeals,
                Message = "Deals found",
                Succeeded = true
            };
        }
        catch (AmazonDynamoDBException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new GetLatestDealsResponseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
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
                    {"PK", new AttributeValue(Key.CategoryPk(name))},
                    {"SK", new AttributeValue(Key.CategorySk(name))}
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
