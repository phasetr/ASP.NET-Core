using Amazon.CDK;
using CdkSes;

var app = new App();
new CdkSesStack(app, "CdkSesStack", new StackProps());
app.Synth();
