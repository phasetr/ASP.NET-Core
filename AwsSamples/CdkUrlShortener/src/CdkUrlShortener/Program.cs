using Amazon.CDK;
using CdkUrlShortener;

var app = new App();
var unused = new CdkUrlShortenerStack(app, "cdk-url-shortener-stack", new StackProps());
app.Synth();
