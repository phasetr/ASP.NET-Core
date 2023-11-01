using Amazon.CDK;

namespace CdkBastion;

public class MyStackProps : StackProps
{
    public MyConfiguration Configuration { get; init; } = default!;
}
