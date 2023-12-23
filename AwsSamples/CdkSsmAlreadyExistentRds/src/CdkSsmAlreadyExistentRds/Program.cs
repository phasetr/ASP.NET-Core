using Amazon.CDK;
using CdkSsmAlreadyExistentRds;

var app = new App();
var _ = new CdkSsmAlreadyExistentRdsStack(app, "CdkSsmAlreadyExistentRdsStack");
app.Synth();
