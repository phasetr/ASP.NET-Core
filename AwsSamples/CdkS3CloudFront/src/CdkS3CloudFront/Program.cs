using Amazon.CDK;
using CdkS3CloudFront;

var app = new App();
var unused = new CdkS3CloudFrontStack(app, "CdkS3CloudFrontStack");
app.Synth();
