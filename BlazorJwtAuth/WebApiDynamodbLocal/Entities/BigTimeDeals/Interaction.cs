using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Interaction : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(Interaction);
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public string Name { get; set; } = default!;

    public override string ToPk()
    {
        return $"{Type.ToUpper()}#{Name.ToLower()}#{UserName.ToLower()}";
    }

    public override string ToSk()
    {
        return $"{Type.ToUpper()}#{Name.ToLower()}#{UserName.ToLower()}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"Type", new AttributeValue(Type)},
            {"UserName", new AttributeValue(UserName)},
            {"Name", new AttributeValue(Name)}
        };
    }
}

public class Like : Interaction
{
    [DynamoDBProperty] public new string Type { get; set; } = nameof(Like);
}

public class Watch : Interaction
{
    [DynamoDBProperty] public new string Type { get; set; } = nameof(Watch);

    public override string ToPk()
    {
        return $"{Type.ToUpper()}#{Name.ToLower()}";
    }

    public override string ToSk()
    {
        return $"USER#{UserName.ToLower()}";
    }
}

public class BrandLike : Like
{
    [DynamoDBProperty] public new string Name { get; set; } = nameof(BrandLike);
}

public class BrandWatch : Watch
{
    [DynamoDBProperty] public new string Name { get; set; } = nameof(BrandWatch);
}

public class CategoryLike : Like
{
    [DynamoDBProperty] public new string Name { get; set; } = nameof(CategoryLike);
}

public class CategoryWatch : Watch
{
    [DynamoDBProperty] public new string Name { get; set; } = nameof(CategoryWatch);
}
