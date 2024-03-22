using Amazon.CDK;
using Cdk;

var app = new App();
var _ = new CdkStack(app, "lrp-stack");

app.Synth();
