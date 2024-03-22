using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Stage = Amazon.CDK.AWS.APIGateway.Stage;
using StageProps = Amazon.CDK.AWS.APIGateway.StageProps;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace Cdk;

public class CdkStack : Stack
{
    internal CdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // Lambda関数の定義
        var aspNetCoreFunction = new Function(this, "AspNetCoreFunction", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            Code = Code.FromAsset(".", new AssetOptions
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
                        " && dotnet lambda package -pl Web --output-package /asset-output/function.zip"
                    ]
                }
            }),
            Handler = "Web::Web.LambdaEntryPoint::FunctionHandlerAsync",
            MemorySize = 512,
            Timeout = Duration.Seconds(30),
            Role = new Role(this, "AspNetCoreFunctionRole", new RoleProps
            {
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
                ManagedPolicies =
                [
                    ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaBasicExecutionRole"),
                    ManagedPolicy.FromAwsManagedPolicyName("AWSLambda_FullAccess")
                ]
            }),
            Environment = new Dictionary<string, string>()
        });

        // API GatewayのRestApiの定義
        var api = new RestApi(this, "AspNetCoreApi", new RestApiProps
        {
            RestApiName = $"{Aws.STACK_NAME}-api",
            Description = "API for ASP.NET Core Lambda",
            BinaryMediaTypes = ["*/*"]
        });

        var apiRoot = api.Root.AddResource("{proxy+}");
        apiRoot.AddMethod("ANY", new LambdaIntegration(aspNetCoreFunction));

        // API Gatewayのデプロイメントとステージの定義
        var deployment = new Deployment(this, "AspNetCoreApiDeployment", new DeploymentProps
        {
            Api = api,
            Description = "RestApi deployment"
        });

        var prodStage = new Stage(this, "ProdStage", new StageProps
        {
            Deployment = deployment,
            StageName = "Prod"
        });

        // 出力
        var unused101 = new CfnOutput(this, "ApiURL", new CfnOutputProps
        {
            Description = "API endpoint URL for Prod environment",
            Value = api.Url
        });
    }
}
