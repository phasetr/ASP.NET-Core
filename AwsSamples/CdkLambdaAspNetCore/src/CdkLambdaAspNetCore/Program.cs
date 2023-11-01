using Amazon.CDK;
using CdkLambdaAspNetCore;

const string stackName = "cdk-lambda-asp-net-core-stack";

var app = new App();
var _ = new CdkLambdaAspNetCoreStack(app, $"{stackName}-dev", new MyStackProps
{
    MyConfiguration = new MyConfiguration
    {
        EnvironmentName = "dev"
    },
    Env = new Environment
    {
        Account = "573143736992",
        Region = "ap-northeast-1"
    }
});
app.Synth();
