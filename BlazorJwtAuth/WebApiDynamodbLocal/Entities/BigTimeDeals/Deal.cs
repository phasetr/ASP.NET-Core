using System.Globalization;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using KsuidDotNet;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Deal : BaseEntity
{
    [DynamoDBProperty(AttributeName = "GSI1PK")]
    public string Gsi1Pk = default!;

    [DynamoDBProperty(AttributeName = "GSI1SK")]
    public string Gsi1Sk = default!;

    [DynamoDBProperty(AttributeName = "GSI2PK")]
    public string Gsi2Pk = default!;

    [DynamoDBProperty(AttributeName = "GSI2SK")]
    public string Gsi2Sk = default!;

    [DynamoDBProperty(AttributeName = "GSI3PK")]
    public string Gsi3Pk = default!;

    [DynamoDBProperty(AttributeName = "GSI3SK")]
    public string Gsi3Sk = default!;

    [DynamoDBProperty] public string Type { get; set; } = nameof(Deal);
    [DynamoDBProperty] public string DealId { get; set; } = default!;
    [DynamoDBProperty] public string Title { get; set; } = default!;
    [DynamoDBProperty] public string Link { get; set; } = default!;
    [DynamoDBProperty] public decimal Price { get; set; }
    [DynamoDBProperty] public string Category { get; set; } = default!;
    [DynamoDBProperty] public string Brand { get; set; } = default!;
    [DynamoDBProperty] public DateTime CreatedAt { get; set; }

    public override string ToPk()
    {
        return $"DEAL#{DealId}";
    }

    public override string ToSk()
    {
        return $"DEAL#{DealId}";
    }

    public static string GenerateDealId(DateTime createdAt)
    {
        return Ksuid.NewKsuid(createdAt);
    }

    public static string ToPk(string dealId)
    {
        return $"DEAL#{dealId}";
    }

    public static string ToSk(string dealId)
    {
        return $"DEAL#{dealId}";
    }
    
    public static string ToGsi2Pk(string brandName, DateOnly dateOnly)
    {
        return $"BRAND#{brandName.ToUpper()}#{dateOnly.ToString("yyyy-MM-dd")}";
    }

    public static string ToGsi2Sk(string brandName, DateOnly dateOnly)
    {
        return $"BRAND#{brandName.ToUpper()}#{dateOnly.ToString("yyyy-MM-dd")}";
    }

    public string ToGsi1Pk()
    {
        return $"DEALS#{CreatedAt:yyyy-MM-dd}";
    }

    public string ToGsi1Sk()
    {
        return $"DEAL#{DealId}";
    }

    public string ToGsi2Pk()
    {
        return $"BRAND#{Brand.ToUpper()}#{CreatedAt:yyyy-MM-dd}";
    }

    public string ToGsi2Sk()
    {
        return $"DEAL#{DealId}";
    }

    public string ToGsi3Pk()
    {
        return $"CATEGORY#{Category.ToUpper()}#{CreatedAt:yyyy-MM-dd}";
    }

    public string ToGsi3Sk()
    {
        return $"DEAL${DealId}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"GSI1PK", new AttributeValue(ToGsi1Pk())},
            {"GSI1SK", new AttributeValue(ToGsi1Sk())},
            {"GSI2PK", new AttributeValue(ToGsi2Pk())},
            {"GSI2SK", new AttributeValue(ToGsi2Sk())},
            {"GSI3PK", new AttributeValue(ToGsi3Pk())},
            {"GSI3SK", new AttributeValue(ToGsi3Sk())},
            {"Type", new AttributeValue(Type)},
            {"DealId", new AttributeValue(DealId)},
            {"Title", new AttributeValue(Title)},
            {"Link", new AttributeValue(Link)},
            {"Price", new AttributeValue {N = Price.ToString(CultureInfo.InvariantCulture)}},
            {"Category", new AttributeValue(Category)},
            {"Brand", new AttributeValue(Brand)},
            {"CreatedAt", new AttributeValue(CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"))}
        };
    }
}
