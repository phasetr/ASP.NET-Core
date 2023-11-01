using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Constructs;
using InstanceType = Amazon.CDK.AWS.EC2.InstanceType;

namespace CdkRdsSsm;

public class CdkRdsSsmStack : Stack
{
    private const string Prefix = "cdk-rds-ssm";

    internal CdkRdsSsmStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // VPC
        var vpc = new Vpc(this, $"{Prefix}-vpc", new VpcProps
        {
            NatGatewayProvider = NatProvider.Instance(new NatInstanceProps
            {
                InstanceType = InstanceType.Of(InstanceClass.T3, InstanceSize.MICRO)
            }),
            SubnetConfiguration = new ISubnetConfiguration[]
            {
                new SubnetConfiguration
                {
                    Name = $"{Prefix}-db-subnet",
                    SubnetType = SubnetType.PRIVATE_ISOLATED,
                    CidrMask = 28
                },
                new SubnetConfiguration
                {
                    Name = $"{Prefix}-public-subnet",
                    SubnetType = SubnetType.PUBLIC,
                    CidrMask = 24
                }
            }
        });

        // RDS
        // Subnet Group
        var subnetGroup = new SubnetGroup(this, $"{Prefix}-subnet-group", new SubnetGroupProps
        {
            Description = $"{Prefix} subnet group description",
            Vpc = vpc,
            SubnetGroupName = $"{Prefix}-subnet-group-name",
            VpcSubnets = new SubnetSelection
            {
                OnePerAz = true,
                SubnetType = SubnetType.PRIVATE_ISOLATED
            }
        });
        // DB Cluster
        const string clusterIdentifier = $"{Prefix}-db-cluster";
        var database = new DatabaseCluster(this, $"{Prefix}-default", new DatabaseClusterProps
        {
            Engine = DatabaseClusterEngine.AuroraPostgres(new AuroraPostgresClusterEngineProps
                {Version = AuroraPostgresEngineVersion.VER_14_6}),
            ServerlessV2MaxCapacity = 1,
            ServerlessV2MinCapacity = 0.5,
            ClusterIdentifier = clusterIdentifier,
            Credentials = Credentials.FromGeneratedSecret("pgadmin", new CredentialsBaseOptions
            {
                SecretName = $"{clusterIdentifier}/pgadmin"
            }),
            DefaultDatabaseName = "mydb",
            Writer = ClusterInstance.ServerlessV2($"{Prefix}-writer"),
            Readers = new[]
            {
                ClusterInstance.ServerlessV2($"{Prefix}-reader1", new ServerlessV2ClusterInstanceProps
                {
                    ScaleWithWriter = true
                })
            },
            RemovalPolicy = RemovalPolicy.DESTROY,
            Vpc = vpc,
            SubnetGroup = subnetGroup
        });

        var bastion = new BastionHostLinux(this, $"{Prefix}-bastion", new BastionHostLinuxProps
        {
            MachineImage = MachineImage.LatestAmazonLinux2(),
            Vpc = vpc,
            SubnetSelection = new SubnetSelection
            {
                SubnetType = SubnetType.PUBLIC
            }
        });
        database.Connections.AllowDefaultPortFrom(bastion);
    }
}
