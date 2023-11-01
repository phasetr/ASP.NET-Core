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

namespace CdkSamBlazorAspNetCoreDynamoDb;

public class CdkSamBlazorAspNetCoreDynamoDbStack : Stack
{
    private const string Prefix = "cdk-sam-blazor-asp-net-core-dynamodb";

    internal CdkSamBlazorAspNetCoreDynamoDbStack(Construct scope, string id, MyStackProps props = null) : base(scope,
        id,
        props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        // Lambda
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
        var lambda = new Function(this, $"{Prefix}-lambda-asp-net-core-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 256,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "ServerlessApi",
            Code = Code.FromAsset("ServerlessApi/", new AssetOptions
            {
                Bundling = buildOption
            })
        });

        // API Gateway
        // ReSharper disable once UnusedVariable
        var restApi = new LambdaRestApi(this, $"{Prefix}-api-gw-{envName}", new LambdaRestApiProps
        {
            Handler = lambda,
            Proxy = true
        });

        // DynamoDB
        var dynamodb = new Table(this, $"{Prefix}-dynamodb-{envName}", new TableProps
        {
            PartitionKey = new Attribute
            {
                Name = "PK",
                Type = AttributeType.STRING
            },
            ReadCapacity = 3,
            WriteCapacity = 3,
            RemovalPolicy = RemovalPolicy.DESTROY
        });
        // Grant the lambda role read/write permissions to our table
        dynamodb.GrantReadWriteData(lambda);

        // S3
        var contentsBucket = new Bucket(this, $"{Prefix}-s3-{envName}", new BucketProps
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
                    Name = $"{Prefix}-oac-for-bucket-{envName}",
                    OriginAccessControlOriginType = "s3",
                    SigningBehavior = "always",
                    SigningProtocol = "sigv4",
                    Description = "Access Control for S3 Bucket"
                }
            });
        // CloudFront
        var cloudFront = new Distribution(this, $"{Prefix}-cloudfront-{envName}", new DistributionProps
        {
            Comment = "distribution for bucket",
            DefaultBehavior = new BehaviorOptions
            {
                Origin = new S3Origin(contentsBucket),
                AllowedMethods = AllowedMethods.ALLOW_ALL
            },
            DefaultRootObject = "index.html",
            HttpVersion = HttpVersion.HTTP2_AND_3
        });
        // S3 bucket policy statement
        var contentsBucketPolicyStatement = new PolicyStatement(new PolicyStatementProps
        {
            Actions = new[] {"s3:GetObject"},
            Effect = Effect.ALLOW,
            Principals = new IPrincipal[] {new ServicePrincipal("cloudfront.amazonaws.com")},
            Resources = new[] {contentsBucket.ArnForObjects("*")}
        });
        contentsBucketPolicyStatement.AddCondition("StringEquals", new Dictionary<string, object>
        {
            {
                "AWS:SourceArn",
                $"arn:aws:cloudfront::{Aws.ACCOUNT_ID}:distribution/cloudfront/{cloudFront.DistributionId}-{envName}"
            }
        });
        contentsBucket.AddToResourcePolicy(contentsBucketPolicyStatement);

        // Outputs
        var unused101 = new CfnOutput(this, $"{Prefix}-api-gw-tarn-{envName}",
            new CfnOutputProps {Value = restApi.ArnForExecuteApi()});
        var unused102 = new CfnOutput(this, $"{Prefix}-api-gw-url-{envName}",
            new CfnOutputProps {Value = restApi.UrlForPath()});
        var unused103 = new CfnOutput(this, $"{Prefix}-s3-bucket-name-{envName}",
            new CfnOutputProps {Value = contentsBucket.BucketName});
        var unused104 = new CfnOutput(this, $"{Prefix}-cloudfront-domain-name-{envName}",
            new CfnOutputProps {Value = cloudFront.DistributionDomainName});
    }
}
