namespace Cdk.Common;

public static class Constants
{
    private const string Prefix = "cdk-blazor-dotnet8";
    private static readonly string PrefixWithNoHyphens = Prefix.Replace("-", "");

    public const string StackName = $"{Prefix}-stack";
    public const string DynamoDbDevTableName = $"{Prefix}-dynamodb-dev";
    public const string DynamoDbLocalUrl = "http://localhost:8000";
    public const string DynamoDbLocalTableName = $"{Prefix}-dynamodb-local";

    public const string DynamoDb = $"{Prefix}-dynamodb";
    public const string DynamoDbPartitionKey = "PartitionKey";
    public const string DynamoDbSortKey = "SortKey";
    public const string Lambda = $"{Prefix}-lambda";
    public const string ApiGateway = $"{Prefix}-api-gw";

    public static readonly string OutputApiGwArn = $"{PrefixWithNoHyphens}apigwarn";
    public static readonly string OutputApiGwUrl = $"{PrefixWithNoHyphens}apigwurl";
    public static readonly string OutputDynamoDbTableName = $"{PrefixWithNoHyphens}dynamodbtablename";
    public static readonly string OutputDynamoDbUrl = $"{PrefixWithNoHyphens}dynamodburl";
}
