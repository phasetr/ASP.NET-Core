using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Page : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(Page);
    [DynamoDBProperty] public string FeaturedDeals { get; set; } = default!;

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.PagePk())},
            {"SK", new AttributeValue(Key.PageSk())},
            {"Type", new AttributeValue(Type)},
            {"FeaturedDeals", new AttributeValue(FeaturedDeals)}
        };
    }
}

public class FrontPage : Page
{
}

public class EditorsChoice : Page
{
}
