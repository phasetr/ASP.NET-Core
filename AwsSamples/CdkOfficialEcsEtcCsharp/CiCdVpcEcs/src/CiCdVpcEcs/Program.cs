using Amazon.CDK;
using CiCdVpcEcs;

var app = new App();
var unused = new CiCdVpcEcsStack(app, "cdk-official-ecs-etc-csharp-ci-cd-vpc-ecs-stack");
app.Synth();
