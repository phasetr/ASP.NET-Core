using System;
using System.Collections.Generic;
using System.Web;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.Cognito;
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
    private const string Prefix = "cdk-fullstack-with-auth";

    internal CdkStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        #region BlazorFrontend

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

        #endregion

        #region "Cognito User Pool"

        // Create Cognito User Pool with Email Sign-in.
        var userPool = new UserPool(this, $"{Prefix}-cognito-user-pool-{envName}", new UserPoolProps
        {
            UserPoolName = $"{Prefix}-cognito-user-pool-{envName}",
            RemovalPolicy = RemovalPolicy.DESTROY,
            SignInAliases = new SignInAliases {Email = true},
            SelfSignUpEnabled = false
        });
        // Create Cognito user group
        var unused5 = new CfnUserPoolGroup(this, $"{Prefix}-user-pool-group-read-only-{envName}",
            new CfnUserPoolGroupProps
            {
                UserPoolId = userPool.UserPoolId,
                Description = "Read Only Access",
                GroupName = "read-only"
            });
        // Create Cognito user group
        var unused6 = new CfnUserPoolGroup(this, $"{Prefix}-user-pool-group-read-update-add-{envName}",
            new CfnUserPoolGroupProps
            {
                UserPoolId = userPool.UserPoolId,
                Description = "Full Access",
                GroupName = "read-update-add"
            });
        // Create Cognito App Client
        var cognitoAppClient = new UserPoolClient(this, $"{Prefix}-cognito-app-client-{envName}",
            new UserPoolClientProps
            {
                UserPoolClientName = $"{Prefix}-cognito-app-client-{envName}",
                UserPool = userPool,
                AccessTokenValidity = Duration.Minutes(20),
                IdTokenValidity = Duration.Minutes(20),
                RefreshTokenValidity = Duration.Hours(1),
                AuthFlows = new AuthFlow
                {
                    UserPassword = true,
                    UserSrp = true
                },
                OAuth = new OAuthSettings
                {
                    CallbackUrls = new[] {"https://localhost:6500", $"https://{cloudFront.DistributionDomainName}"},
                    Flows = new OAuthFlows {ImplicitCodeGrant = true},
                    Scopes = new[] {OAuthScope.EMAIL, OAuthScope.OPENID},
                    LogoutUrls = new[]
                    {
                        "https://localhost:6500/authentication/logout-callback",
                        $"https://{cloudFront.DistributionDomainName}/authentication/logout-callback"
                    }
                }
            });
        // Create Domain name for Cognito Hosted UI
        var cognitoDomainName = new UserPoolDomain(this, $"{Prefix}-cognito-domain-name-{envName}",
            new UserPoolDomainProps
            {
                UserPool = userPool,
                CognitoDomain = new CognitoDomainOptions
                    {DomainPrefix = "sign-in-" + cognitoAppClient.UserPoolClientId}
            });

        #endregion

        #region DynamoDB

        // DynamoDB
        var dynamodb = new Table(this, $"{Prefix}-dynamodb-{envName}", new TableProps
        {
            TableName = $"{Prefix}-dynamodb-{envName}",
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
        var lambda = new Function(this, $"{Prefix}-lambda-{envName}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "Api",
            Environment = new Dictionary<string, string>
            {
                {"CLIENT_URL", $"https://{cloudFront.DistributionDomainName}"},
                {"REGION", Region},
                {"COGNITO_USER_POOL_ID", userPool.UserPoolId},
                {"CLIENT_ID", cognitoAppClient.UserPoolClientId},
                {"TABLE_NAME", dynamodb.TableName}
            },
            Code = Code.FromAsset(".", new AssetOptions
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
                        " && dotnet lambda package --project-location Api --output-package /asset-output/function.zip"
                    }
                }
            })
        });
        // Grant the lambda role read/write permissions to our table
        dynamodb.GrantReadWriteData(lambda);

        // API Gateway
        var apiGateway = new LambdaRestApi(this, $"{Prefix}-api-gw-{envName}", new LambdaRestApiProps
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

        var unused101 = new CfnOutput(this, $"{Prefix}-api-gw-arn-{envName}",
            new CfnOutputProps {Value = apiGateway.ArnForExecuteApi()});
        var unused102 = new CfnOutput(this, $"{Prefix}-api-gw-url-{envName}",
            new CfnOutputProps {Value = apiGateway.UrlForPath()});
        var unused104 = new CfnOutput(this, $"{Prefix}-cloudfront-domain-name-{envName}",
            new CfnOutputProps {Value = $"https://{cloudFront.DistributionDomainName}"});
        var unused103 = new CfnOutput(this, $"{Prefix}-s3-bucket-name-{envName}",
            new CfnOutputProps {Value = contentsBucket.BucketName});

        var unused7 = new CfnOutput(this, $"{Prefix}-cognito-app-client-id-{envName}",
            new CfnOutputProps {Value = cognitoAppClient.UserPoolClientId});
        var unused4 = new CfnOutput(this, $"{Prefix}-cognito-hosted-ui-url-root",
            new CfnOutputProps
            {
                Value =
                    $"{cognitoDomainName.BaseUrl()}/login?response_type=token&client_id={cognitoAppClient.UserPoolClientId}&redirect_uri="
            });
        var unused8 = new CfnOutput(this, $"{Prefix}-cognito-user-pool-domain-{envName}",
            new CfnOutputProps {Value = cognitoDomainName.DomainName});
        var unused9 = new CfnOutput(this, $"{Prefix}-cognito-user-pool-id-{envName}",
            new CfnOutputProps {Value = userPool.UserPoolId});

        #endregion
    }
}