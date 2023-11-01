using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace CdkLambdaByCsharp;

public class CdkLambdaByCsharpStack : Stack
{
    private new const string StackName = "cdk-lambda-by-csharp-stack";

    internal CdkLambdaByCsharpStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
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

        var lambdaFunctionOne = new Function(this, $"{StackName}-my-func-one", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "FunctionOne",
            Code = Code.FromAsset("lambda/src/FunctionOne/", new AssetOptions
            {
                Bundling = buildOption
            })
        });

        var lambdaFunctionTwo = new Function(this, $"{StackName}-my-func-two", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "FunctionTwo",
            Code = Code.FromAsset("lambda/src/FunctionTwo/", new AssetOptions
            {
                Bundling = buildOption
            })
        });

        var lambdaFunctionThree = new Function(this, $"{StackName}-my-func-three", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "FunctionThree",
            Code = Code.FromAsset("lambda/src/FunctionThree/", new AssetOptions
            {
                Bundling = buildOption
            })
        });

        // Proxy all request from the root path "/" to Lambda Function One
        var restApi = new LambdaRestApi(this, $"{StackName}-end-point", new LambdaRestApiProps
        {
            Handler = lambdaFunctionOne,
            Proxy = true
        });

        // Proxy all request from path "/function-two" to Lambda Function Two
        // この指定は`FunctionTwo/Program.cs`の`app.UsePathBase(new PathString("/function-two"));`にしたがう
        var apiFunctionTwo = restApi.Root.AddResource("function-two", new ResourceOptions
        {
            DefaultIntegration = new LambdaIntegration(lambdaFunctionTwo)
        });
        apiFunctionTwo.AddMethod("ANY");
        apiFunctionTwo.AddProxy();

        // Proxy all request from path "/function-three" to Lambda Function Three
        // この指定は`FunctionThree/Program.cs`の`app.UsePathBase(new PathString("/function-three"));`にしたがう
        var apiFunctionThree = restApi.Root.AddResource("function-three", new ResourceOptions
        {
            DefaultIntegration = new LambdaIntegration(lambdaFunctionThree)
        });
        apiFunctionThree.AddMethod("ANY");
        apiFunctionThree.AddProxy();

        var unused = new CfnOutput(this, $"{StackName}-api-gw-tarn",
            new CfnOutputProps {Value = restApi.ArnForExecuteApi()});
    }
}
