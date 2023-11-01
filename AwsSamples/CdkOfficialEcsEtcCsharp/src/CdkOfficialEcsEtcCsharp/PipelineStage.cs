using Amazon.CDK;
using Constructs;

namespace CdkOfficialEcsEtcCsharp;

public class PipelineStage : Stage
{
    public PipelineStage(Construct scope, string id, IStageProps props = null) : base(scope, id, props)
    {
        var unused = new CdkOfficialEcsEtcCsharpStack(this, "cdk-official-ecs-etc-csharp-stack");
    }
}
