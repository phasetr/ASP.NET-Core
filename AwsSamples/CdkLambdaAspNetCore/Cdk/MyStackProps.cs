using Amazon.CDK;

namespace CdkLambdaAspNetCore;

public class MyStackProps : StackProps
{
    public MyConfiguration MyConfiguration { get; init; } = default!;
}
