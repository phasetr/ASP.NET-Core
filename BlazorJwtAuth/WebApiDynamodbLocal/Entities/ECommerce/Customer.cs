using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto;

namespace WebApiDynamodbLocal.Entities.ECommerce;

[DynamoDBTable(AwsSettings.ECommerceTable)]
public class Customer : BaseEntity
{
    public new const string EntityName = "Customer";
    [DynamoDBProperty] public string Type { get; set; } = default!;
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public string Email { get; set; } = default!;
    [DynamoDBProperty] public string Name { get; set; } = default!;
    [DynamoDBProperty] public Dictionary<string, Address> Addresses { get; set; } = default!;

    /// <summary>
    /// TODO: ToPkとToSkを作ろう
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string UserNameToPk(string userName)
    {
        return $"{nameof(Customer).ToUpper()}#{userName}";
        // return $"{EntityName.ToUpper()}#{userName}";
    }

    public override EntityKey Key()
    {
        return new EntityKey
        {
            Pk = UserNameToPk(UserName),
            Sk = UserNameToPk(UserName)
        };
    }

    public override BaseEntity ToItem()
    {
        var key = Key();
        return new Customer
        {
            Pk = key.Pk,
            Sk = key.Sk,
            Type = nameof(Customer),
            UserName = UserName,
            Email = Email,
            Name = Name,
            Addresses = Addresses
        };
    }

    public Dictionary<string, AttributeValue> PostDtoToDynamoDbItem(PostCustomerDto postCustomerDto)
    {
        var key = UserNameToPk(postCustomerDto.UserName);
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(key)},
            {"SK", new AttributeValue(key)},
            {"Type", new AttributeValue(nameof(Customer))},
            {"UserName", new AttributeValue(postCustomerDto.UserName)},
            {"Email", new AttributeValue(postCustomerDto.Email)},
            {"Name", new AttributeValue(postCustomerDto.Name)},
            {"Addresses", new AttributeValue {M = AddressesToDynamoDbItem()}}
        };
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        var key = Key();
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(key.Pk)},
            {"SK", new AttributeValue(key.Sk)},
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
