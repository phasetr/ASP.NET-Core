using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Cdk.Common;
using Constructs;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;

namespace Cdk;

public sealed class CdkStack : Stack
{
    internal CdkStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var envName = configuration.EnvironmentName;

        #region ECR

        var ecrRepository = new Repository(this, $"{Constants.EcrRepositoryName}-{envName}",
            new RepositoryProps
            {
                AutoDeleteImages = true,
                LifecycleRules = new ILifecycleRule[] {new LifecycleRule {MaxImageCount = 1}},
                RepositoryName = $"{Constants.EcrRepositoryName}-{envName}",
                RemovalPolicy = RemovalPolicy.DESTROY
            });

        #endregion

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

        // Lambda from ECR
        var lambda = new DockerImageFunction(this, $"{Constants.LambdaFromEcr}-{envName}", new DockerImageFunctionProps
        {
            Code = DockerImageCode.FromEcr(ecrRepository),
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            Environment = new Dictionary<string, string>
            {
                {"REGION", Region},
                {"TABLE_NAME", dynamodb.TableName}
            }
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
        var unused104 = new CfnOutput(this, $"{Constants.OutputEcrRepositoryName}{envName}",
            new CfnOutputProps {Value = ecrRepository.RepositoryName});

        #endregion
    }
}
