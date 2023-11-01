using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.AWS.CodeCommit;
using Amazon.CDK.AWS.CodePipeline;
using Amazon.CDK.AWS.CodePipeline.Actions;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ECS.Patterns;
using Amazon.CDK.AWS.IAM;
using Constructs;
using IStageProps = Amazon.CDK.AWS.CodePipeline.IStageProps;
using StageProps = Amazon.CDK.AWS.CodePipeline.StageProps;

namespace CdkOfficialEcsEtcCsharp;

public sealed class PipelineStack : Stack
{
    public PipelineStack(Construct parent, string id, IStackProps props = null) : base(parent, id, props)
    {
        // Get the microservice name from context
        const string microServiceName = "cdk-official-ecs-etc-csharp";

        // Get the code commit parameters from context
        const string repositoryName = $"{microServiceName}";
        const string repositoryConstrId = $"{microServiceName}-codecommit-id";
        const string branchName = "master";
        const string repositoryDescription = $"Repository for {microServiceName}";
        const string s3BucketForCode = $"{microServiceName}-add-code";
        const string s3ObjectKeyForCode = "starter-code.zip";

        const string pipelineId = $"{microServiceName}-pipeline-id";

        // Get ecr parameters from context
        const string ecrRepositoryConstrId = $"{microServiceName}-ecrrepo-id";
        const string ecrRepositoryName = $"{microServiceName}";
        const string imageTag = "latest";

        // Get code build parameters from context
        const string codeBuildConstrId = $"{microServiceName}-codebuild-id";
        const string codeBuildProjectName = $"{microServiceName}";

        // Get ecs fargate service parameters from context
        const string ecsServiceName = $"{microServiceName}";
        const string ecsServiceRoleContrId = $"{microServiceName}-ecsrole-id";
        const string ecsServiceRoleName = $"{microServiceName}-ecs-taskexecution-role";
        const string ecsFargateConstrId = $"{microServiceName}-ecs-fargate-id";
        const string ecsFargateConstrIdProd = $"{microServiceName}-prod-ecs-fargate-id";

        // Get code pipeline parameters from context
        const string pipelineConstrId = $"{microServiceName}-codepipeline-id";
        const string pipelineName = $"{microServiceName}";

        // Create the repository and add the starter code. The starter code is available in the S3 bucket defined in
        // cdk.json

        var cfnResource = new CfnRepository(this, repositoryConstrId, new CfnRepositoryProps
        {
            RepositoryName = repositoryName
        });
        cfnResource.AddPropertyOverride("Code.BranchName", branchName);
        cfnResource.AddPropertyOverride("Code.S3.Bucket", s3BucketForCode);
        cfnResource.AddPropertyOverride("Code.S3.Key", s3ObjectKeyForCode);
        cfnResource.AddPropertyOverride("RepositoryDescription", repositoryDescription);

        // Get handle to code commit repository object to use in code pipeline source action
        var codeCommitRepo = Repository.FromRepositoryName(this, "Repository", repositoryName);
        // var codeCommitRepo = new Repository(this, repositoryConstrId, new RepositoryProps
        // {
        //     RepositoryName = repositoryName
        // });

        // Get handle to the ecr repository to use by code build
        var ecrRepo = new Amazon.CDK.AWS.ECR.Repository(this, ecrRepositoryConstrId,
            new Amazon.CDK.AWS.ECR.RepositoryProps
            {
                RepositoryName = ecrRepositoryName,
                RemovalPolicy = RemovalPolicy.DESTROY
            });

        // Create the starter ECS Fargate Service using AWS provided public nginx image. This will be updated later
        // with the built image by the pipeline
        var starterImage = ContainerImage.FromRegistry(
            "public.ecr.aws/b4f2s5k2/project-demo-reinvent/nginx-web-app:latest");
        var executionPolicy = ManagedPolicy.FromAwsManagedPolicyName("service-role/AmazonECSTaskExecutionRolePolicy");
        var executionRole = new Role(
            this,
            ecsServiceRoleContrId,
            new RoleProps
            {
                AssumedBy = new ServicePrincipal("ecs-tasks.amazonaws.com"),
                ManagedPolicies = new[] {executionPolicy},
                RoleName = ecsServiceRoleName
            });

        // CAUTION：`CiCdVpcEcs`を新たに`cdk deploy`したら書き換えが必要
        const string vpcNonProdId = "vpc-02e4f489c6859887b";
        const string vpcProdId = "vpc-03a98bfd797bf4992";
        const string ecsSgNonProdId = "sg-0c31969197bb1aaa8";
        const string ecsSgProdId = "sg-05da32edc08348188";
        /*
        const string vpcNonProdId = $"{microServiceName}-vpc-nonprod-id";
        const string vpcProdId = $"{microServiceName}-vpc-prod-id";
        const string ecsSgNonProdId = $"{microServiceName}-ecssg-nonprod-id";
        const string ecsSgProdId = $"{microServiceName}-ecssg-prod-id";
        */
        
        const string ecsNonProdName = $"{microServiceName}-ci-cd-ecs-nonprod";
        const string ecsProdName = $"{microServiceName}-ci-cd-ecs-prod";

        // Added by me
        const string vpcDbSubnetGroupName = $"{microServiceName}-db-subnet-group-name";
        const string vpcPublicSubnetGroupName = $"{microServiceName}-public-subnet-group-name";
        const string vpcAppSubnetGroupName = $"{microServiceName}-cdk-app-subnet-group-name";
        const string vpcConnectorId = $"{microServiceName}-vpc-connector-id";
        const string databaseId = $"{microServiceName}-database-id";

        var vpcNonProd = Vpc.FromLookup(this, "vpd-nonprod", new VpcLookupOptions
        {
            VpcId = vpcNonProdId
        });
        var vpcProd = Vpc.FromLookup(this, "vpc-prod", new VpcLookupOptions
        {
            VpcId = vpcProdId
        });

        var ecsSgNonProd = SecurityGroup.FromSecurityGroupId(this, "ecssg-nonprod", ecsSgNonProdId);
        var ecsSgProd = SecurityGroup.FromSecurityGroupId(this, "ecssg-prod", ecsSgProdId);

        var ecsNonProd = Cluster.FromClusterAttributes(this, "ecs-nonprod", new ClusterAttributes
        {
            ClusterName = ecsNonProdName,
            Vpc = vpcNonProd,
            SecurityGroups = new[] {ecsSgNonProd}
        });
        var ecsProd = Cluster.FromClusterAttributes(this, "ecs-prod", new ClusterAttributes
        {
            ClusterName = ecsProdName,
            Vpc = vpcProd,
            SecurityGroups = new[] {ecsSgProd}
        });

        var albFargateService = new ApplicationLoadBalancedFargateService(this, ecsFargateConstrId,
            new ApplicationLoadBalancedFargateServiceProps
            {
                TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                {
                    Image = starterImage,
                    ContainerName = "app",
                    ExecutionRole = executionRole
                },
                DesiredCount = 2,
                ServiceName = ecsServiceName,
                ListenerPort = 80,
                Cluster = ecsNonProd
            });
        var fargateService = albFargateService.Service;

        var albFargateServiceProd = new ApplicationLoadBalancedFargateService(this, ecsFargateConstrIdProd,
            new ApplicationLoadBalancedFargateServiceProps
            {
                TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                {
                    Image = starterImage,
                    ContainerName = "app",
                    ExecutionRole = executionRole
                },
                DesiredCount = 2,
                ServiceName = ecsServiceName,
                ListenerPort = 80,
                Cluster = ecsProd
            });
        var fargateServiceProd = albFargateServiceProd.Service;

        // Create the CodeBuild project that creates the Docker image, and pushes it to the ecr repository
        var codeBuildProject = new PipelineProject(this, codeBuildConstrId, new PipelineProjectProps
        {
            ProjectName = codeBuildProjectName,
            Environment = new BuildEnvironment
            {
                BuildImage = LinuxBuildImage.STANDARD_7_0,
                Privileged = true,
                EnvironmentVariables = new Dictionary<string, IBuildEnvironmentVariable>
                {
                    {"AWS_ACCOUNT_ID", new BuildEnvironmentVariable {Value = Account}},
                    {"AWS_DEFAULT_REGION", new BuildEnvironmentVariable {Value = Region}},
                    {"IMAGE_REPO_NAME", new BuildEnvironmentVariable {Value = ecrRepositoryName}},
                    {"IMAGE_TAG", new BuildEnvironmentVariable {Value = imageTag}},
                    {"REPOSITORY_URL", new BuildEnvironmentVariable {Value = ecrRepo.RepositoryUri}},
                    {"REPOSITORY_URI", new BuildEnvironmentVariable {Value = ecrRepo.RepositoryUri}}
                }
            },
            BuildSpec = BuildSpec.FromObject(new Dictionary<string, object>
            {
                {
                    "version", "0.2"
                },
                {
                    "phases", new Dictionary<string, object>
                    {
                        {
                            "pre_build", new Dictionary<string, object>
                            {
                                {
                                    "commands", new[]
                                    {
                                        "echo Logging in to Amazon ECR...",
                                        "aws --version",
                                        "aws ecr get-login-password --region $AWS_DEFAULT_REGION | docker login --username AWS --password-stdin $AWS_ACCOUNT_ID.dkr.ecr.$AWS_DEFAULT_REGION.amazonaws.com"
                                    }
                                }
                            }
                        },
                        {
                            "build", new Dictionary<string, object>
                            {
                                {
                                    "commands", new[]
                                    {
                                        "echo Build started on `date`",
                                        "echo Building the Docker image...",
                                        "pwd",
                                        "ls",
                                        "cd WebApi/WebApi",
                                        "docker build -t $IMAGE_REPO_NAME:$IMAGE_TAG --target final .",
                                        "docker tag $IMAGE_REPO_NAME:$IMAGE_TAG $AWS_ACCOUNT_ID.dkr.ecr.$AWS_DEFAULT_REGION.amazonaws.com/$IMAGE_REPO_NAME:$IMAGE_TAG"
                                    }
                                }
                            }
                        },
                        {
                            "post_build", new Dictionary<string, object>
                            {
                                {
                                    "commands", new[]
                                    {
                                        "echo Build completed on `date`",
                                        "echo Pushing the Docker image...",
                                        "pwd",
                                        "cd ../../",
                                        "pwd",
                                        "ls",
                                        "docker push $AWS_ACCOUNT_ID.dkr.ecr.$AWS_DEFAULT_REGION.amazonaws.com/$IMAGE_REPO_NAME:$IMAGE_TAG",
                                        "echo $CODEBUILD_RESOLVED_SOURCE_VERSION",
                                        "export imageTag=$CODEBUILD_RESOLVED_SOURCE_VERSION",
                                        "echo $imageTag",
                                        "printf '[{\"name\":\"app\",\"imageUri\":\"%s\"}]' $IMAGE_REPO_NAME:$imageTag > imagedefinitions.json",
                                        "ls"
                                    }
                                }
                            }
                        }
                    }
                },
                {
                    "env", new Dictionary<string, object> {{"exported-variables", new[] {"imageTag"}}}
                },
                {
                    "artifacts", new Dictionary<string, object>
                    {
                        {"files", new[] {"imagedefinitions.json"}},
                        {
                            "secondary-artifacts", new Dictionary<string, object>
                            {
                                {
                                    "imagedefinitions",
                                    new Dictionary<string, object>
                                        {{"files", new[] {"imagedefinitions.json"}}, {"name", "imagedefinitions"}}
                                }
                            }
                        }
                    }
                }
            })
        });
        // Grant push/pull permissions on ecr repo to code build project needed for `docker push`
        ecrRepo.GrantPullPush(codeBuildProject);

        // Define the source action for code pipeline
        var sourceOutput = new Artifact_();
        var sourceAction = new CodeCommitSourceAction(new CodeCommitSourceActionProps
        {
            ActionName = "CodeCommit",
            Branch = branchName,
            Repository = codeCommitRepo,
            Output = sourceOutput,
            CodeBuildCloneOutput = true
        });
        // Define the build action for code pipeline
        var buildAction = new CodeBuildAction(new CodeBuildActionProps
        {
            ActionName = "CodeBuild",
            Project = codeBuildProject,
            Input = sourceOutput,
            Outputs = new[] {new Artifact_("imagedefinitions")},
            ExecuteBatchBuild = false
        });

        // Define the deploy action for code pipeline
        var deployAction = new EcsDeployAction(new EcsDeployActionProps
        {
            ActionName = "DeployECS",
            Service = fargateService,
            Input = new Artifact_("imagedefinitions")
        });

        var manualApprovalProd = new ManualApprovalAction(new ManualApprovalActionProps
        {
            ActionName = "Approve-Prod-Deploy",
            RunOrder = 1
        });

        var deployActionProd = new EcsDeployAction(new EcsDeployActionProps
        {
            ActionName = "DeployECS",
            Service = fargateServiceProd,
            Input = new Artifact_("imagedefinitions"),
            RunOrder = 2
        });

        //  Create the pipeline
        var unused1 = new Pipeline(this, pipelineConstrId, new PipelineProps
        {
            PipelineName = pipelineName,
            Stages = new IStageProps[]
            {
                new StageProps {StageName = "Source", Actions = new IAction[] {sourceAction}},
                new StageProps {StageName = "Build", Actions = new IAction[] {buildAction}},
                new StageProps {StageName = "Deploy-NonProd", Actions = new IAction[] {deployAction}},
                new StageProps
                    {StageName = "Deploy-Prod", Actions = new IAction[] {manualApprovalProd, deployActionProd}}
            }
        });

        // VPC
        // var vpc = new Vpc(this, vpcProdId, new VpcProps
        // {
        //     NatGatewayProvider = NatProvider.Instance(new NatInstanceProps
        //     {
        //         InstanceType = InstanceType.Of(InstanceClass.T3, InstanceSize.NANO)
        //     }),
        //     SubnetConfiguration = new ISubnetConfiguration[]
        //     {
        //         new SubnetConfiguration
        //         {
        //             Name = vpcDbSubnetGroupName,
        //             SubnetType = SubnetType.PRIVATE_ISOLATED,
        //             CidrMask = 28
        //         },
        //         new SubnetConfiguration
        //         {
        //             Name = vpcPublicSubnetGroupName,
        //             SubnetType = SubnetType.PUBLIC,
        //             CidrMask = 24
        //         },
        //         new SubnetConfiguration
        //         {
        //             Name = vpcAppSubnetGroupName,
        //             SubnetType = SubnetType.PRIVATE_WITH_EGRESS,
        //             CidrMask = 24
        //         }
        //     }
        // });
        // var vpcConnector = new VpcConnector(this, VpcConnectorId, new VpcConnectorProps
        // {
        //     Vpc = vpc,
        //     VpcSubnets = new SubnetSelection {SubnetGroupName = VpcDbSubnetGroupName}
        // });

        // App Runner
        // var describeImagesPolicy = new PolicyStatement(new PolicyStatementProps
        // {
        //     Actions = new[] {"ecr:DescribeImages"},
        //     Resources = new[] {"*"}
        // });
        // var appRunnerService = new Service(this, AppRunnerId, new ServiceProps
        // {
        //     // AccessRole = new Role(this, "CdkAppRunnerInstanceRole", new RoleProps
        //     // {
        //     //     AssumedBy = new ServicePrincipal("build.apprunner.amazonaws.com"),
        //     //     InlinePolicies = new Dictionary<string, PolicyDocument>
        //     //     {
        //     //         ["DescribeImagesPolicy"] = new(new PolicyDocumentProps
        //     //         {
        //     //             Statements = new[] {describeImagesPolicy}
        //     //         })
        //     //     }
        //     // }),
        //     AutoDeploymentsEnabled = true,
        //     ServiceName = AppRunnerServiceName,
        //     Cpu = Cpu.QUARTER_VCPU,
        //     Memory = Memory.ONE_GB,
        //     Source = Source.FromEcr(new EcrProps
        //     {
        //         Repository = ecrRepository,
        //         ImageConfiguration = new ImageConfiguration
        //         {
        //             Port = 80
        //         }
        //     })
        //     // VpcConnector = vpcConnector
        // });
        // 上のAccessRoleを外したときの対処
        // var cfnAccessRole = appRunnerService.Node.FindChild("AccessRole").Node.DefaultChild as CfnRole;
        // var unused4 = new CfnPolicy(this, "CfnPolicy", new CfnPolicyProps
        // {
        //     PolicyName = "AppRunnerAccessRoleDescribeImages",
        //     PolicyDocument = new Dictionary<string, object>
        //     {
        //         ["Version"] = "2012-10-17",
        //         ["Statement"] = new object[]
        //         {
        //             new Dictionary<string, object>
        //             {
        //                 ["Effect"] = "Allow",
        //                 ["Action"] = new[]
        //                 {
        //                     "ecr:DescribeImages"
        //                 },
        //                 ["Resource"] = new[]
        //                 {
        //                     "*"
        //                 }
        //             }
        //         }
        //     },
        //     Roles = new[]
        //     {
        //         cfnAccessRole!.Ref
        //     }
        // });
        // var unused3 = new AwsCustomResource(this, "CdkAppRunnerRdsCustomResource", new AwsCustomResourceProps
        // {
        //     OnCreate = new AwsSdkCall
        //     {
        //         Service = "AppRunner",
        //         Action = "associateCustomDomain",
        //         Parameters = new Dictionary<string, object>
        //         {
        //             {"ServiceArn", appRunnerService.ServiceArn},
        //             {"DomainName", "academic-event.com"},
        //             {"EnableWWWSubdomain", false}
        //         },
        //         PhysicalResourceId = PhysicalResourceId.Of("CdkAppRunnerRdsCustomResource")
        //     },
        //     OnDelete = new AwsSdkCall
        //     {
        //         Service = "AppRunner",
        //         Action = "disassociateCustomDomain",
        //         Parameters = new Dictionary<string, object>
        //         {
        //             {"ServiceArn", appRunnerService.ServiceArn},
        //             {"DomainName", "academic-event.com"}
        //         },
        //         PhysicalResourceId = PhysicalResourceId.Of("CdkAppRunnerRdsCustomResource")
        //     },
        //     Policy = AwsCustomResourcePolicy.FromSdkCalls(new SdkCallsPolicyOptions
        //     {
        //         Resources = new[] {appRunnerService.ServiceArn}
        //     })
        // });
        // var unused2 = new CfnOutput(this, AppRunnerUrl, new CfnOutputProps
        // {
        //     ExportName = AppRunnerUrl,
        //     Value = appRunnerService.ServiceUrl
        // });
// 
        // RDS
        // const string suffix = "cdk-app-runner-rds";
        // const string clusterIdentifier = "cdk-app-runner-rds-db-cluster";
        // // DB Cluster
        // var database = new DatabaseCluster(this, databaseId, new DatabaseClusterProps
        // {
        //     Engine = DatabaseClusterEngine.AuroraPostgres(new AuroraPostgresClusterEngineProps
        //         {Version = AuroraPostgresEngineVersion.VER_14_6}),
        //     ServerlessV2MaxCapacity = 1,
        //     ServerlessV2MinCapacity = 0.5,
        //     ClusterIdentifier = clusterIdentifier,
        //     Credentials = Credentials.FromGeneratedSecret("pgadmin", new CredentialsBaseOptions
        //     {
        //         SecretName = $"{clusterIdentifier}/pgadmin"
        //     }),
        //     DefaultDatabaseName = "mydb",
        //     Writer = ClusterInstance.ServerlessV2($"{suffix}-Writer"),
        //     Readers = new[]
        //     {
        //         ClusterInstance.ServerlessV2($"{suffix}-Reader1", new ServerlessV2ClusterInstanceProps
        //         {
        //             ScaleWithWriter = true
        //         })
        //     },
        //     RemovalPolicy = RemovalPolicy.DESTROY,
        //     Vpc = vpc
        //     // SubnetGroup = subnetGroup
        // });
        // // database.Connections.AllowDefaultPortFrom(vpcConnector);
        // var instanceRole = new Role(this, "InstanceRole", new RoleProps
        // {
        //     AssumedBy = new ServicePrincipal("tasks.apprunner.amazonaws.com")
        // });
        // database.Secret?.GrantRead(instanceRole);
// 
        // var bastion = new BastionHostLinux(this, "Bastion", new BastionHostLinuxProps
        // {
        //     MachineImage = MachineImage.LatestAmazonLinux2(),
        //     Vpc = vpc,
        //     SubnetSelection = new SubnetSelection
        //     {
        //         SubnetGroupName = vpcAppSubnetGroupName
        //     }
        // });
        // database.Connections.AllowDefaultPortFrom(bastion);
// 
        // // Define the source action for code pipeline
        // var sourceOutput = new Artifact_();
        // var sourceAction = new CodeCommitSourceAction(new CodeCommitSourceActionProps
        // {
        //     ActionName = "CodeCommit",
        //     Branch = "main",
        //     Repository = codeCommitRepo,
        //     Output = sourceOutput,
        //     CodeBuildCloneOutput = true
        // });
        // // Define the build action for code pipeline
        // var buildAction = new CodeBuildAction(new CodeBuildActionProps
        // {
        //     ActionName = "CodeBuild",
        //     Project = codeBuildProject,
        //     Input = sourceOutput,
        //     Outputs = new[] {Artifact_.Artifact("imagedefinitions")},
        //     ExecuteBatchBuild = false
        // });
        // // Define the deploy action for code pipeline
// 
        // // Create the pipeline
        // var pipeline = new Pipeline(this, pipelineId, new PipelineProps
        // {
        //     PipelineName = "CdkAppRunnerRdsPipeline"
        // });
        // pipeline.AddStage(new StageOptions
        // {
        //     StageName = "Source",
        //     Actions = new IAction[] {sourceAction}
        // });
        // pipeline.AddStage(new StageOptions
        // {
        //     StageName = "Build",
        //     Actions = new IAction[] {buildAction}
        // });
    }
}
