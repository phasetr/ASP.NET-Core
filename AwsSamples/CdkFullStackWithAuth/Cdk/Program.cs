using Amazon.CDK;
using Cdk;

const string stackName = "cdk-fullstack-with-auth-stack";

var app = new App();
var unused1 = new CdkStack(app, $"{stackName}-dev", new MyStackProps
{
    MyConfiguration = new MyConfiguration
    {
        EnvironmentName = "dev"
    }
});
app.Synth();
