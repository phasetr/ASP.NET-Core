namespace CdkBastion;

public class MyConfiguration
{
    public string EnvironmentName { get; init; } = "dev";
    public string DbClusterArn { get; init; } = default!;
    public string VpcId { get; init; } = default!;
    public string SecurityGroupId { get; init; } = default!;
    public string Cidr { get; init; } = default!;
}
