using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace CdkS3CloudFront;

public class CdkS3CloudFrontStack : Stack
{
    internal CdkS3CloudFrontStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // S3
        var contentsBucket = new Bucket(this, "Bucket", new BucketProps
        {
            BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
            RemovalPolicy = RemovalPolicy.DESTROY
        });
        // OAC
        var cfnOriginAccessControl = new CfnOriginAccessControl(this, "OriginAccessControl",
            new CfnOriginAccessControlProps
            {
                OriginAccessControlConfig = new CfnOriginAccessControl.OriginAccessControlConfigProperty
                {
                    Name = "OriginAccessControlForBucket",
                    OriginAccessControlOriginType = "s3",
                    SigningBehavior = "always",
                    SigningProtocol = "sigv4",
                    Description = "Access Control for S3 Bucket"
                }
            });

        // CloudFront
        var distribution = new Distribution(this, "Distribution", new DistributionProps
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
                $"arn:aws:cloudfront::{Aws.ACCOUNT_ID}:distribution/cloudfront/{distribution.DistributionId}"
            }
        });
        contentsBucket.AddToResourcePolicy(contentsBucketPolicyStatement);

        // 出力
        var unused2 = new CfnOutput(this, "S3BucketName", new CfnOutputProps
        {
            Value = contentsBucket.BucketName
        });
        var unused3 = new CfnOutput(this, "CloudFrontDomainName", new CfnOutputProps
        {
            Value = distribution.DomainName
        });
    }
}
