using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.S3;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;
using Function = Amazon.CDK.AWS.Lambda.Function;
using FunctionProps = Amazon.CDK.AWS.Lambda.FunctionProps;

namespace Cdk;

public sealed class CdkStack : Stack
{
    private const string Prefix = "ba";

    internal CdkStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        #region BlazorFrontend

        // S3
        var frontendS3 = new Bucket(this, $"{Prefix}-s3-{envName}", new BucketProps
        {
            BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
            RemovalPolicy = RemovalPolicy.DESTROY,
            AutoDeleteObjects = true
        });
        // OAC
        var unused1 = new CfnOriginAccessControl(this, $"{Prefix}-oac-{envName}",
            new CfnOriginAccessControlProps
            {
                OriginAccessControlConfig = new CfnOriginAccessControl.OriginAccessControlConfigProperty
                {
                    Name = $"{Prefix}-oac-bucket-{envName}",
                    OriginAccessControlOriginType = "s3",
                    SigningBehavior = "always",
                    SigningProtocol = "sigv4",
                    Description = "Access Control for S3 Bucket"
                }
            });
        // CloudFront
        var frontendCloudFront = new Distribution(this, $"{Prefix}-cf-{envName}", new DistributionProps
        {
            Comment = "distribution for bucket",
            DefaultBehavior = new BehaviorOptions
            {
                Origin = new S3Origin(frontendS3),
                AllowedMethods = AllowedMethods.ALLOW_ALL
            },
            DefaultRootObject = "index.html",
            HttpVersion = HttpVersion.HTTP2_AND_3
        });
        // S3 bucket policy statement
        var frontendS3PolicyStatement = new PolicyStatement(new PolicyStatementProps
        {
            Actions = ["s3:GetObject"],
            Effect = Effect.ALLOW,
            Principals = [new ServicePrincipal("cloudfront.amazonaws.com")],
            Resources = [frontendS3.ArnForObjects("*")]
        });
        frontendS3PolicyStatement.AddCondition("StringEquals", new Dictionary<string, object>
        {
            {
                "AWS:SourceArn",
                $"arn:aws:cloudfront::{Aws.ACCOUNT_ID}:distribution/cloudfront/{frontendCloudFront.DistributionId}-{envName}"
            }
        });
        frontendS3.AddToResourcePolicy(frontendS3PolicyStatement);

        #endregion

        #region DynamoDB

        // DynamoDB
        var dynamodb = new Table(this, $"{Prefix}-ddb-{envName}", new TableProps
        {
            TableName = $"{Prefix}-ddb-{envName}",
            RemovalPolicy = RemovalPolicy.DESTROY,
            PartitionKey = new Attribute
            {
                Name = "PK",
                Type = AttributeType.STRING
            },
            SortKey = new Attribute
            {
                Name = "SK",
                Type = AttributeType.STRING
            },
            ReadCapacity = 3,
            WriteCapacity = 3
        });

        #endregion

        #region LambdaApiGateway

        // Lambda
        var lambda = new Function(this, $"{Prefix}-l-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "Api",
            Environment = new Dictionary<string, string>
            {
                { "FRONTEND_URL", $"https://{frontendCloudFront.DistributionDomainName}" },
                { "REGION", Region },
                { "TABLE_NAME", dynamodb.TableName }
            },
            Code = Code.FromAsset("Api", new AssetOptions
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
        // Grant the lambda role read/write permissions to our table
        dynamodb.GrantReadWriteData(lambda);

        // API Gateway
        var backendApiGateway = new LambdaRestApi(this, $"{Prefix}-ag-{envName}", new LambdaRestApiProps
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

        var unused101 = new CfnOutput(this, $"{Prefix}-b-ag-arn-{envName}",
            new CfnOutputProps { Value = backendApiGateway.ArnForExecuteApi() });
        var unused102 = new CfnOutput(this, $"{Prefix}-b-ag-url-{envName}",
            new CfnOutputProps { Value = backendApiGateway.UrlForPath() });
        var unused104 = new CfnOutput(this, $"{Prefix}-f-dn-{envName}",
            new CfnOutputProps { Value = $"https://{frontendCloudFront.DistributionDomainName}" });
        var unused103 = new CfnOutput(this, $"{Prefix}-f-bn-{envName}",
            new CfnOutputProps { Value = frontendS3.BucketName });
        var unused105 = new CfnOutput(this, $"{Prefix}-ddb-tn-{envName}",
            new CfnOutputProps { Value = dynamodb.TableName });

        #endregion
    }
}
