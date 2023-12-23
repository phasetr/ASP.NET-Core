using Amazon.CDK;
using CdkAiRelated;

const string stackName = "cdk-ai-related-stack";

var app = new App();
var _ = new CdkAiRelated.CdkAiRelated(app, $"{stackName}-dev", new MyStackProps
{
    MyConfiguration = new MyConfiguration
    {
        EnvironmentName = "dev"
    }
});
app.Synth();
