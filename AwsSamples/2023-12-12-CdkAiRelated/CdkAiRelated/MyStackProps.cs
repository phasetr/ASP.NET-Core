using Amazon.CDK;

namespace CdkAiRelated;

public class MyStackProps : StackProps
{
    public MyConfiguration MyConfiguration { get; init; } = default!;
}
