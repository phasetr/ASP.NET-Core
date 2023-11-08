using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Interaction : IEntity
{
    public string Type { get; set; } = nameof(Interaction);
    public string UserName { get; set; } = default!;
    public string Name { get; set; } = default!;

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.InteractionPk(Type, Name, UserName))},
            {"SK", new AttributeValue(Key.InteractionSk(Type, Name, UserName))},
            {"Type", new AttributeValue(Type)},
            {"UserName", new AttributeValue(UserName)},
            {"Name", new AttributeValue(Name)}
        };
    }
}

public class Like : Interaction
{
    public new string Type { get; set; } = nameof(Like);
}

public class Watch : Interaction
{
    public new string Type { get; set; } = nameof(Watch);
}

public class BrandLike : Like
{
    public new string Name { get; set; } = nameof(BrandLike);
}

public class BrandWatch : Watch
{
    public new string Name { get; set; } = nameof(BrandWatch);
}

public class CategoryLike : Like
{
    public new string Name { get; set; } = nameof(CategoryLike);
}

public class CategoryWatch : Watch
{
    public new string Name { get; set; } = nameof(CategoryWatch);
}
