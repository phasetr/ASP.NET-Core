using Amazon.CDK;
using CdkS3CloudFrontDevProd;

var app = new App();

// Development
var unused1 = new CdkS3CloudFrontDevProdStack(app, "cdk-s3-cloudfront-dev-prod-stack-dev", new MyStackProps
{
    Configuration = new Configuration
    {
        EnvironmentName = "dev"
    },
    Env = new Environment
    {
        Account = "573143736992",
        Region = "ap-northeast-1"
    }
});
// Production
var unused2 = new CdkS3CloudFrontDevProdStack(app, "cdk-s3-cloudfront-dev-prod-stack-prod", new MyStackProps
{
    Configuration = new Configuration
    {
        EnvironmentName = "prod"
    },
    Env = new Environment
    {
        Account = "573143736992",
        Region = "ap-northeast-1"
    }
});
app.Synth();
