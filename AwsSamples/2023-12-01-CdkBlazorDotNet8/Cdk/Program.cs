using Amazon.CDK;
using Cdk;

const string stackName = "cdk-blazor-dotnet8-stack";

var app = new App();
var unused1 = new CdkStack(app, $"{stackName}-dev", new MyStackProps
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
