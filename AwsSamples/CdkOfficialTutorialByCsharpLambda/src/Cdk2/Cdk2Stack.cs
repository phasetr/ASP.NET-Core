using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace Cdk2;

public class Cdk2Stack : Stack
{
    internal Cdk2Stack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        var helloLambda = new Function(this, "HelloHandler", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("./lambda/HelloHandler/src/HelloHandler/bin/Release/net6.0/publish"),
            Handler = "HelloHandler::HelloHandler.Function::FunctionHandler"
        });

        var unused1 = new LambdaRestApi(this, "HelloApi", new LambdaRestApiProps
        {
            Handler = helloLambda
        });

        var unused2 = new Bucket(this, "MyFirstBucket", new BucketProps
        {
            Versioned = true,
            RemovalPolicy = RemovalPolicy.DESTROY,
            AutoDeleteObjects = true
        });
    }
}
