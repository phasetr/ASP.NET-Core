using Amazon.CDK;
using CdkEc2ApiServer;

const string stackName = "cdk-ec2-api-server";
var app = new App();
var unused1 = new CdkEc2ApiServerStack(app, $"{stackName}-dev", new MyStackProps
{
    MyConfiguration = new MyConfiguration
    {
        StackName = $"{stackName}-dev"
    }
});
app.Synth();
