using Amazon.CDK;
using CdkOfficialEcsEtcCsharp;

var app = new App();
var unused = new PipelineStack(app, "cdk-official-ecs-etc-csharp-stack", new StackProps
{
    Env = new Environment
    {
        Account = "573143736992",
        Region = "ap-northeast-1"
    }
});
app.Synth();
