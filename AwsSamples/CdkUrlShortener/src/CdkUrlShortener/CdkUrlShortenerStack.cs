using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace CdkUrlShortener;

public class CdkUrlShortenerStack : Stack
{
    private new const string StackName = "cdk-url-shortener-stack";

    internal CdkUrlShortenerStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        var buildOption = new BundlingOptions
        {
            Image = Runtime.DOTNET_6.BundlingImage,
            User = "root",
            OutputType = BundlingOutput.ARCHIVED,
            Command = new[]
            {
                "/bin/sh",
                "-c",
                " dotnet tool install -g Amazon.Lambda.Tools" +
                " && dotnet build" +
                " && dotnet lambda package --output-package /asset-output/function.zip"
            }
        };

        var lambdaFunction = new Function(this, $"{StackName}-lambda-function", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "Function",
            Code = Code.FromAsset("lambda/Function/", new AssetOptions
            {
                Bundling = buildOption
            })
        });

        // キーが短縮URL、バリューがもとのURL
        var table = new Table(this, $"{StackName}-mapping-table", new TableProps
        {
            PartitionKey = new Attribute
            {
                Name = "id",
                Type = AttributeType.STRING
            },
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        table.GrantReadWriteData(lambdaFunction);
        lambdaFunction.AddEnvironment("TABLE_NAME", table.TableName);

        // Proxy all request from the root path "/" to Lambda Function One
        var restApi = new LambdaRestApi(this, $"{StackName}-url-shortener-api", new LambdaRestApiProps
        {
            Handler = lambdaFunction,
            Proxy = true
        });

        var unused = new CfnOutput(this, $"{StackName}-api-gw-arn",
            new CfnOutputProps {Value = restApi.ArnForExecuteApi()});
    }
}
