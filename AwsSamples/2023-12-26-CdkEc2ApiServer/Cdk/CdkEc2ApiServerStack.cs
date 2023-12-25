using System;
using System.Collections.Generic;
using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;

namespace CdkEc2ApiServer;

public class CdkEc2ApiServerStack : Stack
{
    internal CdkEc2ApiServerStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.MyConfiguration ?? throw new ArgumentNullException(nameof(props));
        var stackName = configuration.StackName;

        #region EC2

        // ロールの作成とIAMポリシー
        var webServerRole = new Role(this, $"{stackName}-ec2-role", new RoleProps
        {
            AssumedBy = new ServicePrincipal("ec2.amazonaws.com")
        });
        // IAM policy attachment to allow access to
        webServerRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("AmazonSSMManagedInstanceCore"));
        webServerRole.AddManagedPolicy(
            ManagedPolicy.FromAwsManagedPolicyName("service-role/AmazonEC2RoleforAWSCodeDeploy"));

        // VPCを作成する
        var vpc = new Vpc(this, $"{stackName}-vpc", new VpcProps
        {
            SubnetConfiguration = new ISubnetConfiguration[]
            {
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    Name = "pub01",
                    SubnetType = SubnetType.PUBLIC
                }
                // new SubnetConfiguration
                // {
                //     CidrMask = 24,
                //     Name = "pub02",
                //     SubnetType = SubnetType.PUBLIC
                // },
                // new SubnetConfiguration
                // {
                //     CidrMask = 24,
                //     Name = "pub03",
                //     SubnetType = SubnetType.PUBLIC
                // }
            }
        });

        // EC2インスタンスのセキュリティグループを作成する
        var webSg = new SecurityGroup(this, $"{stackName}-sg", new SecurityGroupProps
        {
            Vpc = vpc,
            AllowAllOutbound = true,
            Description = "Allows Inbound HTTP traffic to the web server"
        });
        webSg.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(22), "SSH from anywhere");
        webSg.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(8080), "HTTP from anywhere");

        // The actual Web EC2 Instance for the web server
        var keyPair = new KeyPair(this, $"{stackName}-key-pair", new KeyPairProps
        {
            KeyPairName = $"{stackName}-key",
            Type = KeyPairType.ED25519
        });
        var webServer = new Instance_(this, $"{stackName}-web-server", new InstanceProps
        {
            KeyName = keyPair.KeyPairName,
            // KeyPair = keyPair,
            Vpc = vpc,
            InstanceType = InstanceType.Of(InstanceClass.T3, InstanceSize.MICRO),
            MachineImage = new AmazonLinuxImage(new AmazonLinuxImageProps
            {
                Generation = AmazonLinuxGeneration.AMAZON_LINUX_2,
                CpuType = AmazonLinuxCpuType.X86_64
            }),
            VpcSubnets = new SubnetSelection
            {
                SubnetType = SubnetType.PUBLIC
            },
            SecurityGroup = webSg,
            Role = webServerRole
        });
        // assets/configure_amz_linux_sample_app.shを読み取る
        // ファイルを読み取る
        var webSgUserData = new StreamReader("Cdk/assets/configure_amz_linux_sample_app.sh").ReadToEnd();
        webServer.AddUserData(webSgUserData);

        #endregion

        #region Start/Stop EC2 by Lambda & EventBridge

        // LambdaでEC2を起動・停止させる
        var lambdaRole = new Role(this, $"{stackName}-lambda-role", new RoleProps
        {
            AssumedBy = new ServicePrincipal("lambda.amazonaws.com")
        });
        lambdaRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[]
            {
                "logs:CreateLogGroup",
                "logs:CreateLogStream",
                "logs:PutLogEvents"
            },
            Resources = new[] {"arn:aws:logs:*:*:*"}
        }));
        lambdaRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[]
            {
                "ec2:Start*",
                "ec2:Stop*"
            },
            Resources = new[] {"*"}
        }));
        var lambda = new Function(this, $"{stackName}-lambda-ec2-control", new FunctionProps
        {
            Runtime = Runtime.PYTHON_3_11,
            MemorySize = 256,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "ec2_controller.lambda_handler",
            Code = Code.FromAsset("Cdk"),
            Role = lambdaRole
        });
        // EventBridgeで定期実行する設定を追加
        var unused2 = new Rule(this, $"{stackName}-start-ec2-role", new RuleProps
        {
            Schedule = Schedule.Cron(new CronOptions
            {
                Minute = "0",
                Hour = "9",
                WeekDay = "MON-FRI"
            }),
            Targets = new IRuleTarget[]
            {
                new LambdaFunction(lambda, new LambdaFunctionProps
                {
                    Event = RuleTargetInput.FromObject(new Dictionary<string, dynamic>
                    {
                        {"Action", "start"},
                        {"Region", "ap-northeast-1"},
                        {"Instances", new[] {webServer.InstanceId}}
                    })
                })
            }
        });
        var unused3 = new Rule(this, $"{stackName}-stop-ec2-role", new RuleProps
        {
            Schedule = Schedule.Cron(new CronOptions
            {
                Minute = "30",
                Hour = "18",
                WeekDay = "MON-FRI"
            }),
            Targets = new IRuleTarget[]
            {
                new LambdaFunction(lambda, new LambdaFunctionProps
                {
                    Event = RuleTargetInput.FromObject(new Dictionary<string, dynamic>
                    {
                        {"Action", "stop"},
                        {"Region", "ap-northeast-1"},
                        {"Instances", new[] {webServer.InstanceId}}
                    })
                })
            }
        });

        #endregion

        #region Outputs

        var unused1 = new CfnOutput(this, $"{stackName}-ip-address",
            new CfnOutputProps {Value = webServer.InstancePublicIp});
        var unused4 = new CfnOutput(this, $"{stackName}-instance-id",
            new CfnOutputProps {Value = webServer.InstanceId});
        var unused5 = new CfnOutput(this, $"{stackName}-lambda-name", new CfnOutputProps {Value = lambda.FunctionName});
        var unused6 = new CfnOutput(this, $"{stackName}-lambda-arn", new CfnOutputProps {Value = lambda.FunctionArn});

        #endregion
    }
}
