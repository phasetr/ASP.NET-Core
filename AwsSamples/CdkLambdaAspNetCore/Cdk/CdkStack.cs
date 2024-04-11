using System;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace CdkLambdaAspNetCore;

public class CdkStack : Stack
{
    private const string Prefix = "ls";

    internal CdkStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        // Lambda
        var lambda = new Function(this, $"{Prefix}-l-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 512,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "ServerlessApi",
            Code = Code.FromAsset("ServerlessApi/", new AssetOptions
            {
                Bundling = new BundlingOptions
                {
                    Image = Runtime.DOTNET_8.BundlingImage,
                    User = "root",
                    OutputType = BundlingOutput.ARCHIVED,
                    Command =
                    [
                        "/bin/sh",
                        "-c",
                        " dotnet tool install -g Amazon.Lambda.Tools" +
                        " && dotnet build" +
                        " && dotnet lambda package --output-package /asset-output/function.zip"
                    ]
                }
            })
        });

        // API Gateway
        var restApi = new LambdaRestApi(this, $"{Prefix}-apigw-{envName}", new LambdaRestApiProps
        {
            Handler = lambda,
            Proxy = true
        });

        var unused1 = new CfnOutput(this, $"{Prefix}-apigw-arn-{envName}",
            new CfnOutputProps { Value = restApi.ArnForExecuteApi() });
        var unused2 = new CfnOutput(this, $"{Prefix}-apigw-url-{envName}",
            new CfnOutputProps { Value = restApi.UrlForPath() });
    }
}
