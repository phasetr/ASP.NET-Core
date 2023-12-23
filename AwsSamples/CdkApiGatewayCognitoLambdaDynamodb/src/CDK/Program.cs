using Amazon.CDK;
using ApiGatewayCognitoLambdaDynamodb;
using Environment = Amazon.CDK.Environment;

var app = new App();
_ = new CdkStack(
    app,
    "ApiGatewayAuthStack"
);
app.Synth();
