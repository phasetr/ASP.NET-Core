using Amazon.DynamoDBv2.DataModel;

namespace AuthFunction;

[DynamoDBTable("UserGroupApiGwAccessPolicy")]
public class DynamoDbTableModel
{
    [DynamoDBHashKey] public string UserPoolGroup { get; set; } = string.Empty;
    public string ApiGwAccessPolicy { get; set; } = string.Empty;
}