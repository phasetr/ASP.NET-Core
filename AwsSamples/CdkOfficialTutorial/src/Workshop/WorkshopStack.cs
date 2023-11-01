using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Cdklabs.DynamoTableViewer;
using Constructs;

namespace Workshop;

public class WorkshopStack : Stack
{
    public readonly CfnOutput HCEndpoint;
    public readonly CfnOutput HCViewerUrl;

    public WorkshopStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        var hello = new Function(this, "HelloHandler", new FunctionProps
        {
            Runtime = Runtime.NODEJS_14_X,
            Code = Code.FromAsset("lambda"),
            Handler = "hello.handler"
        });

        var helloWithCounter = new HitCounter(this, "HelloHitCounter", new HitCounterProps
        {
            Downstream = hello
        });

        var gateway = new LambdaRestApi(this, "Endpoint", new LambdaRestApiProps
        {
            Handler = helloWithCounter.Handler
        });

        var tv = new TableViewer(this, "ViewHitCount", new TableViewerProps
        {
            Title = "Hello Hits",
            Table = helloWithCounter.MyTable
        });

        HCViewerUrl = new CfnOutput(this, "TableViewerUrl", new CfnOutputProps
        {
            Value = tv.Endpoint
        });
        HCEndpoint = new CfnOutput(this, "GatewayUrl", new CfnOutputProps
        {
            Value = gateway.Url
        });

        // 削除時にs3のバケットも削除する
        _ = new Bucket(this, "WorkshopBucket", new BucketProps
        {
            RemovalPolicy = RemovalPolicy.DESTROY,
            AutoDeleteObjects = true
        });
    }
}
