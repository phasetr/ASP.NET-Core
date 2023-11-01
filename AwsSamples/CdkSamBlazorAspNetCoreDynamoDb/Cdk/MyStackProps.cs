using Amazon.CDK;

namespace CdkSamBlazorAspNetCoreDynamoDb;

public class MyStackProps : StackProps
{
    public MyConfiguration MyConfiguration { get; init; } = default!;
}
