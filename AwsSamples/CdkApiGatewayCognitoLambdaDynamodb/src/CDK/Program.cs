using Amazon.CDK;
using ApiGatewayCognitoLambdaDynamodb;
using Environment = Amazon.CDK.Environment;

var app = new App();
_ = new CdkStack(
    app,
    "ApiGatewayAuthStack",
    new StackProps {Env = new Environment {Region = "ap-northeast-1"}}
);
app.Synth();