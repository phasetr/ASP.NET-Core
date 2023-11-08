using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.ECommerce;

[DynamoDBTable(AwsSettings.ECommerceTable)]
public class Customer : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(Customer);
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public string Email { get; set; } = default!;
    [DynamoDBProperty] public string Name { get; set; } = default!;
    [DynamoDBProperty] public Dictionary<string, Address> Addresses { get; set; } = default!;

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.CustomerPk(UserName))},
            {"SK", new AttributeValue(Key.CustomerSk(UserName))},
            {"Type", new AttributeValue(nameof(Customer))},
            {"UserName", new AttributeValue(UserName)},
            {"Email", new AttributeValue(Email)},
            {"Name", new AttributeValue(Name)},
            {"Addresses", new AttributeValue {M = AddressesToDynamoDbItem()}}
        };
    }

    private Dictionary<string, AttributeValue> AddressesToDynamoDbItem()
    {
        var addresses = new Dictionary<string, AttributeValue>();
        foreach (var (key, value) in Addresses)
            addresses.Add(key, new AttributeValue {M = value.ToDynamoDbItem()});
        return addresses;
    }
}

public class Address
{
    [DynamoDBProperty] public string StreetAddress { get; set; } = default!;
    [DynamoDBProperty] public string PostalCode { get; set; } = default!;
    [DynamoDBProperty] public string Country { get; set; } = default!;

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"StreetAddress", new AttributeValue(StreetAddress)},
            {"PostalCode", new AttributeValue(PostalCode)},
            {"Country", new AttributeValue(Country)}
        };
    }
}
