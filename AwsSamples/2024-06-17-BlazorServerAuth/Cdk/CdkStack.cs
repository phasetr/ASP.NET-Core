using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.Apigatewayv2;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AwsApigatewayv2Integrations;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;
using Function = Amazon.CDK.AWS.Lambda.Function;
using FunctionProps = Amazon.CDK.AWS.Lambda.FunctionProps;

namespace Cdk;

public sealed class CdkStack : Stack
{
    private const string Prefix = "bs";

    internal CdkStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        #region LambdaApiGateway

        // Lambda
        var blazorLambda = new Function(this, $"{Prefix}-l-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "Api",
            Environment = new Dictionary<string, string>
            {
                { "REGION", Region }
            },
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
                        " dotnet restore" +
                        " && dotnet tool install -g Amazon.Lambda.Tools" +
                        " && dotnet build" +
                        " && dotnet lambda package --project-location 2024-06-17-BlazorServerAuth --output-package /asset-output/function.zip"
                    ]
                }
            })
        });

        // WebSocket APIの作成
        var websocketApi = new WebSocketApi(this, $"{Prefix}-ws-{envName}", new WebSocketApiProps
        {
            ConnectRouteOptions = new WebSocketRouteOptions
            {
                Integration = new WebSocketLambdaIntegration($"{Prefix}-ws-cr-{envName}", blazorLambda)
            },
            DisconnectRouteOptions = new WebSocketRouteOptions
            {
                Integration = new WebSocketLambdaIntegration($"{Prefix}-ws-dcr-{envName}", blazorLambda)
            },
            DefaultRouteOptions = new WebSocketRouteOptions
            {
                Integration = new WebSocketLambdaIntegration($"{Prefix}-ws-dr-{envName}", blazorLambda)
            }
        });

        var unused1 = new WebSocketStage(this, "BlazorWebSocketStage", new WebSocketStageProps
        {
            WebSocketApi = websocketApi,
            StageName = "dev",
            AutoDeploy = true
        });

        #endregion

        #region Outputs

        var unused101 = new CfnOutput(this, "{Prefix}-url", new CfnOutputProps
        {
            Value = websocketApi.ApiEndpoint
        });

        #endregion
    }
}
