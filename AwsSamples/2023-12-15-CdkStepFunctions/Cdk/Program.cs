using Amazon.CDK;
using CdkStepFunctions;

var app = new App();
var unused1 = new CdkStepFunctionsStack(app, "CdkStepFunctionsStack");
app.Synth();
