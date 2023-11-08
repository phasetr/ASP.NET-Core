using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Page : IEntity
{
    public string FeaturedDeals { get; set; } = default!;

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
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
