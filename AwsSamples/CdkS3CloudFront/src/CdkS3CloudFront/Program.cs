using Amazon.CDK;
using CdkS3CloudFront;

var app = new App();
var unused = new CdkS3CloudFrontStack(app, "CdkS3CloudFrontStack", new StackProps
{
    Env = new Environment
    {
        Account = "573143736992",
        Region = "ap-northeast-1"
    }
});
app.Synth();
