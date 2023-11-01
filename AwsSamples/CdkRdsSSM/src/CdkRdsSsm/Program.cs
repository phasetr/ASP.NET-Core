using Amazon.CDK;
using CdkRdsSsm;

var app = new App();
var unused1 = new CdkRdsSsmStack(app, "cdk-rds-ssm-stack", new StackProps());
app.Synth();
