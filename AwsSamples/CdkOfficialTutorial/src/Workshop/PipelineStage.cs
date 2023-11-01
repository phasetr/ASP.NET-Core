using Amazon.CDK;
using Constructs;

namespace Workshop;

public class PipelineStage : Stage
{
    public readonly CfnOutput HCEndpoint;
    public readonly CfnOutput HCViewerUrl;

    public PipelineStage(Construct scope, string id, IStageProps props = null) : base(scope, id, props)
    {
        var service = new WorkshopStack(this, "WebService");
        HCEndpoint = service.HCEndpoint;
        HCViewerUrl = service.HCViewerUrl;
    }
}
