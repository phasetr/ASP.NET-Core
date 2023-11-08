using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Common.Dto;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Brand;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Services.BigTimeDeals;

public class BrandService : IBrandService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly ILogger<BrandService> _logger;
    private readonly string _tableName;

    public BrandService(
        AmazonDynamoDBClient client,
        IConfiguration configuration,
        ILogger<BrandService> logger)
    {
        _client = client;
        _logger = logger;
        _tableName = configuration[AwsSettings.ConfigurationBigTimeDealsTable];
    }

    public async Task<ResponseBaseDto> CreateAsync(Brand brand)
    {
        try
        {
            var brandContainer = new BrandContainer();
            var transactItems = new List<TransactWriteItem>
            {
                new()
                {
                    Put = new Put
                    {
                        ConditionExpression = "attribute_not_exists(PK)",
                        Item = brand.ToDynamoDbItem(),
                        TableName = _tableName
                    }
                },
                new()
                {
                    Update = new Update
                    {
                        Key = new Dictionary<string, AttributeValue>
                        {
                            {"PK", new AttributeValue(brandContainer.ToPk())},
                            {"SK", new AttributeValue(brandContainer.ToSk())}
                        },
                        TableName = _tableName,
                        UpdateExpression = "ADD #brands :brand",
                        ExpressionAttributeNames = new Dictionary<string, string>
                        {
                            {"#brands", "Brands"}
                        },
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                        {
                            {
                                ":brand", new AttributeValue
                                {
                                    SS = new List<string> {brand.Name}
                                }
                            }
                        }
                    }
                }
            };

            var request = new TransactWriteItemsRequest {TransactItems = transactItems};
            await _client.TransactWriteItemsAsync(request);
            return new ResponseBaseDto
            {
                Message = "Brand created successfully",
                Succeeded = true
            };
        }
        catch (TransactionCanceledException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            if (e.ErrorCode == "ConditionalCheckFailedException")
                return new ResponseBaseDto
                {
                    Message = "Brand with this name already exist",
                    Succeeded = false
                };
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<GetResponseDto> GetAsync(string name)
    {
        try
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"PK", new AttributeValue(Brand.ToPk(name))},
                    {"SK", new AttributeValue(Brand.ToSk(name))}
                }
            };
            var response = await _client.GetItemAsync(request);
            // 項目が取れているか確認
            if (response.Item == null || response.Item.Count == 0)
                return new GetResponseDto
                {
                    Message = "Brand does not exist",
                    Succeeded = false
                };
            return new GetResponseDto
            {
                Message = "Brand found",
                Succeeded = true,
                BrandModel = new BrandModel
                {
                    Name = response.Item["Name"].S,
                    LogoUrl = response.Item["LogoUrl"].S,
                    LikeCount = int.Parse(response.Item["LikeCount"].N),
                    WatchCount = int.Parse(response.Item["WatchCount"].N)
                }
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
