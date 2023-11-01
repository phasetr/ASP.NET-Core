using Amazon.CDK;
using Workshop;

var app = new App();
new WorkshopPipelineStack(app, "WorkshopPipelineStack");
app.Synth();
