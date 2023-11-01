using Amazon.CDK;
using CdkLambdaByCsharp;

var app = new App();
var _ = new CdkLambdaByCsharpStack(app, "cdk-lambda-by-csharp-stack", new StackProps());
app.Synth();
