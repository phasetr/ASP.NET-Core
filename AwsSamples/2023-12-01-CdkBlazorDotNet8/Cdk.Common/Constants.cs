namespace Cdk.Common;

public static class Constants
{
    private const string Prefix = "cdk-blazor-dotnet8";
    private static readonly string PrefixWithNoHyphens = Prefix.Replace("-", "");

    public const string StackName = $"{Prefix}-stack";
    public const string DynamoDbTableName = $"{Prefix}-dynamodb";
    public const string DynamoDbDevUrl = "http://localhost:8000";
    public const string DynamoDbDevTableName = $"{Prefix}-dynamodb-dev";

    public const string EcrRepositoryName = $"{Prefix}-ecr-repository";
    public const string DynamoDb = $"{Prefix}-dynamodb";
    public const string DynamoDbPartitionKey = "PartitionKey";
    public const string DynamoDbSortKey = "SortKey";
    public const string LambdaFromEcr = $"{Prefix}-lambda-from-ecr";
    public const string ApiGateway = $"{Prefix}-api-gw";

    public static readonly string OutputApiGwArn = $"{PrefixWithNoHyphens}apigwarn";
    public static readonly string OutputApiGwUrl = $"{PrefixWithNoHyphens}apigwurl";
    public static readonly string OutputDynamoDbTableName = $"{PrefixWithNoHyphens}dynamodbtablename";
    public static readonly string OutputEcrRepositoryName = $"{PrefixWithNoHyphens}ecrrepositoryname";
}
