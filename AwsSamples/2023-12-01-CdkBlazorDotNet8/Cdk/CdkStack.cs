using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Cdk.Common;
using Constructs;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace Cdk;

public sealed class CdkStack : Stack
{
    internal CdkStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        #region DynamoDB

        // DynamoDB
        var dynamodb = new Table(this, $"{Constants.DynamoDb}-{envName}", new TableProps
        {
            TableName = $"{Constants.DynamoDb}-{envName}",
            RemovalPolicy = RemovalPolicy.DESTROY,
            PartitionKey = new Attribute
            {
                Name = Constants.DynamoDbPartitionKey,
                Type = AttributeType.STRING
            },
            SortKey = new Attribute
            {
                Name = Constants.DynamoDbSortKey,
                Type = AttributeType.STRING
            },
            ReadCapacity = 3,
            WriteCapacity = 3
        });

        #endregion

        #region LambdaApiGateway

        // Lambda
        var lambda = new Function(this, $"{Constants.Lambda}-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Environment = new Dictionary<string, string>
            {
                {"TABLE_NAME", dynamodb.TableName}
            },
            Handler = "BlazorDynamoDb",
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
                        "dotnet restore CdkBlazorDotNet8.sln" +
                        " && dotnet tool install -g Amazon.Lambda.Tools" +
                        " && dotnet build" +
                        " && cd BlazorDynamoDb" +
                        " && dotnet lambda package --output-package /asset-output/function.zip"
                    ]
                }
            })
        });

        // Grant the lambda role read/write permissions to our table
        dynamodb.GrantReadWriteData(lambda);

        // API Gateway
        var apiGateway = new LambdaRestApi(this, $"{Constants.ApiGateway}-{envName}", new LambdaRestApiProps
        {
            Handler = lambda,
            Proxy = true,
            DeployOptions = new StageOptions
            {
                StageName = envName,
                ThrottlingBurstLimit = 10,
                ThrottlingRateLimit = 10,
                LoggingLevel = MethodLoggingLevel.INFO,
                MetricsEnabled = true
            }
        });

        #endregion

        #region Outputs

        var unused101 = new CfnOutput(this, $"{Constants.OutputApiGwArn}{envName}",
            new CfnOutputProps {Value = apiGateway.ArnForExecuteApi()});
        var unused102 = new CfnOutput(this, $"{Constants.OutputApiGwUrl}{envName}",
            new CfnOutputProps {Value = apiGateway.UrlForPath()});
        var unused103 = new CfnOutput(this, $"{Constants.OutputDynamoDbTableName}{envName}",
            new CfnOutputProps {Value = dynamodb.TableName});

        #endregion
    }
}
