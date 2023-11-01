using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace CdkS3Blazor;

public class CdkS3BlazorStack : Stack
{
    private const string Prefix = "cdk-s3-blazor";

    internal CdkS3BlazorStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        // S3
        var contentsBucket = new Bucket(this, $"{Prefix}-s3-{envName}", new BucketProps
        {
            BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
            RemovalPolicy = RemovalPolicy.DESTROY,
            AutoDeleteObjects = true
        });
        // OAC
        var unused1 = new CfnOriginAccessControl(this, $"{Prefix}-origin-access-control-{envName}",
            new CfnOriginAccessControlProps
            {
                OriginAccessControlConfig = new CfnOriginAccessControl.OriginAccessControlConfigProperty
                {
                    Name = $"{Prefix}-origin-access-control-for-bucket-{envName}",
                    OriginAccessControlOriginType = "s3",
                    SigningBehavior = "always",
                    SigningProtocol = "sigv4",
                    Description = "Access Control for S3 Bucket"
                }
            });

        // CloudFront
        var distribution = new Distribution(this, $"{Prefix}-cloudfront-{envName}", new DistributionProps
            {
                Comment = "distribution for bucket",
                DefaultBehavior = new BehaviorOptions
                {
                    Origin = new S3Origin(contentsBucket),
                    AllowedMethods = AllowedMethods.ALLOW_ALL
                },
                DefaultRootObject = "index.html",
                HttpVersion = HttpVersion.HTTP2_AND_3
            }
        );
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
                $"arn:aws:cloudfront::{Aws.ACCOUNT_ID}:distribution/cloudfront/{distribution.DistributionId}"
            }
        });
        contentsBucket.AddToResourcePolicy(contentsBucketPolicyStatement);

        var unused101 = new CfnOutput(this, $"{Prefix}-s3-bucket-name-{envName}", new CfnOutputProps
        {
            Value = contentsBucket.BucketName
        });
        var unused102 = new CfnOutput(this, $"{Prefix}-cloudfront-domain-name-{envName}", new CfnOutputProps
        {
            Value = distribution.DistributionDomainName
        });
    }
}
