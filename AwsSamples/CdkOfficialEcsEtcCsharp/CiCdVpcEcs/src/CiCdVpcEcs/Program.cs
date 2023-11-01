using Amazon.CDK;
using CiCdVpcEcs;

var app = new App();
var unused = new CiCdVpcEcsStack(app, "cdk-official-ecs-etc-csharp-ci-cd-vpc-ecs-stack", new StackProps
{
    Env = new Environment
    {
        Account = "573143736992",
        Region = "ap-northeast-1"
    }
});
app.Synth();
