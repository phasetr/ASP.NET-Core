using Amazon.CDK;
using CdkLambdaAspNetCore;

const string stackName = "ls";

var app = new App();
var _ = new CdkStack(app, $"{stackName}-dev", new MyStackProps
{
    MyConfiguration = new MyConfiguration
    {
        EnvironmentName = "dev"
    }
});
app.Synth();
