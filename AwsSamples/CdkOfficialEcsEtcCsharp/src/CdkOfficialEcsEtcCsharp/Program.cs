using Amazon.CDK;
using CdkOfficialEcsEtcCsharp;

var app = new App();
var unused = new PipelineStack(app, "cdk-official-ecs-etc-csharp-stack");
app.Synth();
