using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Page : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(Page);
    [DynamoDBProperty] public string FeaturedDeals { get; set; } = default!;

    public override string ToPk()
    {
        return "PAGE";
    }

    public override string ToSk()
    {
        return "PAGE";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"Type", new AttributeValue(Type)},
            {"FeaturedDeals", new AttributeValue(FeaturedDeals)}
        };
    }
}

public class FrontPage : Page
{
    public override string ToPk()
    {
        return "FRONTPAGE";
    }

    public override string ToSk()
    {
        return "FRONTPAGE";
    }
}

public class EditorsChoice : Page
{
    public override string ToPk()
    {
        return "EDITORPAGE";
    }

    public override string ToSk()
    {
        return "EDITORPAGE";
    }
}
