using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Page : BaseEntity
{
    public string FeaturedDeals { get; set; } = default!;

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.PagePk())},
            {"SK", new AttributeValue(Key.PageSk())},
            {"Type", new AttributeValue(nameof(Page))},
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
