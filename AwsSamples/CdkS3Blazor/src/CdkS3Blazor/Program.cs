using Amazon.CDK;
using CdkS3Blazor;

const string stackName = "cdk-s3-blazor-stack";

var app = new App();
var unused = new CdkS3BlazorStack(app, $"{stackName}-dev", new MyStackProps
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
