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

        // ServerlessApi
        var apiLambda = new Function(this, $"{Prefix}-api-{envName}", new FunctionProps
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
        var apiLambdaAg = new LambdaRestApi(this, $"{Prefix}-api-apigw-{envName}", new LambdaRestApiProps
        {
            Handler = apiLambda,
            Proxy = true
        });

        // Web (Razor Pages)
        var webLambda = new Function(this, $"{Prefix}-web-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 512,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "Web",
            Code = Code.FromAsset("Web/", new AssetOptions
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
        var webLambdaAg = new LambdaRestApi(this, $"{Prefix}-web-apigw-{envName}", new LambdaRestApiProps
        {
            Handler = webLambda,
            Proxy = true
        });

        var unused1 = new CfnOutput(this, $"{Prefix}-api-arn-{envName}",
            new CfnOutputProps { Value = webLambdaAg.ArnForExecuteApi() });
        var unused2 = new CfnOutput(this, $"{Prefix}-api-url-{envName}",
            new CfnOutputProps { Value = webLambdaAg.UrlForPath() });
        var unused3 = new CfnOutput(this, $"{Prefix}-web-arn-{envName}",
            new CfnOutputProps { Value = webLambdaAg.ArnForExecuteApi() });
        var unused4 = new CfnOutput(this, $"{Prefix}-web-url-{envName}",
            new CfnOutputProps { Value = webLambdaAg.UrlForPath() });
    }
}
