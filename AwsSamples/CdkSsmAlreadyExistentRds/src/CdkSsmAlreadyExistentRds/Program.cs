using Amazon.CDK;
using CdkSsmAlreadyExistentRds;

var app = new App();
var _ = new CdkSsmAlreadyExistentRdsStack(app, "CdkSsmAlreadyExistentRdsStack", new StackProps
{
    Env = new Environment
    {
        Account = "573143736992",
        Region = "ap-northeast-1"
    }
});
app.Synth();
