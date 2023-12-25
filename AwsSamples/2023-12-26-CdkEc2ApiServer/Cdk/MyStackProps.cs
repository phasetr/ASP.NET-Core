using Amazon.CDK;

namespace CdkEc2ApiServer;

public class MyStackProps : StackProps
{
    public MyConfiguration MyConfiguration { get; init; } = default!;
}
