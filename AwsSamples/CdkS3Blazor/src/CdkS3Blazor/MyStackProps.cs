using Amazon.CDK;

namespace CdkS3Blazor;

public class MyStackProps : StackProps
{
    public MyConfiguration MyConfiguration { get; init; } = default!;
}
