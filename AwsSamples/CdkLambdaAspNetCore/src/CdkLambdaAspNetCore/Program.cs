using Amazon.CDK;
using CdkLambdaAspNetCore;

const string stackName = "cdk-lambda-asp-net-core-stack";

var app = new App();
var _ = new CdkLambdaAspNetCoreStack(app, $"{stackName}-dev", new MyStackProps
{
    MyConfiguration = new MyConfiguration
    {
        EnvironmentName = "dev"
    }
});
app.Synth();
