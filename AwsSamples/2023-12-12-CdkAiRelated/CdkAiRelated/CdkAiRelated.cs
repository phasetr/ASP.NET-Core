using System;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace CdkAiRelated;

public class CdkAiRelated : Stack
{
    private const string Prefix = "cdk-ai-related-stack";

    internal CdkAiRelated(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        // HelloWorldLambdaによるサンプル：CLIでの実行法はREADMEを参照
        var helloWorldLambdaFunction = new Function(this, $"{Prefix}-lambda-hello-world-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 256,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "HelloWorldLambda::HelloWorldLambda.Function::FunctionHandler",
            Code = Code.FromAsset("HelloWorldLambda/src/HelloWorldLambda", new AssetOptions
            {
                Bundling = new BundlingOptions
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
                }
            })
        });

        // Lambda
        var lambdaRole = new Role(this, $"{Prefix}-lambda-role-{envName}", new RoleProps
        {
            AssumedBy = new ServicePrincipal("lambda.amazonaws.com")
        });
        lambdaRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] {"ssm:GetParameter", "ssm:GetParameters", "ssm:GetParametersByPath"},
            Resources = new[] {"arn:aws:ssm:ap-northeast-1:573143736992:parameter/OPENAI_API_KEY"}
        }));
        var lambda = new Function(this, $"{Prefix}-serverless-api-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 256,
            LogRetention = RetentionDays.ONE_DAY,
            // Handler = "LambdaBedrock",
            Handler = "HelloWorldLambda::HelloWorldLambda.Function::FunctionHandler",
            // Code = Code.FromAsset("LambdaBedrock/", new AssetOptions
            Code = Code.FromAsset("HelloWorldLambda/src/HelloWorldLambda", new AssetOptions
            {
                Bundling = new BundlingOptions
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
                }
            }),
            Role = lambdaRole,
            Timeout = Duration.Minutes(2)
        });

        // API Gateway
        var restApi = new LambdaRestApi(this, $"{Prefix}-api-gw-{envName}", new LambdaRestApiProps
        {
            Handler = lambda,
            Proxy = true
        });

        var unused1 = new CfnOutput(this, $"{Prefix}-api-gw-arn-{envName}",
            new CfnOutputProps {Value = restApi.ArnForExecuteApi()});
        var unused2 = new CfnOutput(this, $"{Prefix}-api-gw-url-{envName}",
            new CfnOutputProps {Value = restApi.UrlForPath()});
        var unused3 = new CfnOutput(this, $"{Prefix}-hello-world-lambda-fn-name",
            new CfnOutputProps {Value = helloWorldLambdaFunction.FunctionName});
    }
}
