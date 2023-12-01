using Amazon.CDK;

namespace Cdk;

public class MyStackProps : StackProps
{
    public MyConfiguration MyConfiguration { get; init; } = default!;
}
