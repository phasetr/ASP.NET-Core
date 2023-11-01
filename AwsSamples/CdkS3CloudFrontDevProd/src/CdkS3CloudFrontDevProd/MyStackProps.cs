using Amazon.CDK;

namespace CdkS3CloudFrontDevProd;

public class MyStackProps : StackProps
{
    public Configuration Configuration { get; init; } = default!;
}
