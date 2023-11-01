using System;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Constructs;

namespace CdkBastion;

public class CdkBastionStack : Stack
{
    internal CdkBastionStack(Construct scope, string id, MyStackProps props = null) : base(scope, id, props)
    {
        var configuration = props?.Configuration ?? throw new ArgumentNullException(nameof(props));
        var prefix = $"copilot-pipeline-migration-cdk-bastion-{configuration.EnvironmentName}";
        var dbClusterArn = configuration.DbClusterArn;
        var vpcId = configuration.VpcId;
        var securityGroupId = configuration.SecurityGroupId;
        var cidr = configuration.Cidr;

        // VPC
        var vpc = Vpc.FromLookup(this, $"{prefix}-vpc", new VpcLookupOptions
        {
            VpcId = vpcId
        });

        // copilotで作ったDB Clusterを取得
        var database = DatabaseCluster.FromDatabaseClusterAttributes(this, $"{prefix}-db-cluster",
            new DatabaseClusterAttributes
            {
                ClusterIdentifier = dbClusterArn,
                Port = 5432
            });

        var bastion = new BastionHostLinux(this, $"{prefix}-bastion", new BastionHostLinuxProps
        {
            MachineImage = MachineImage.LatestAmazonLinux2(),
            Vpc = vpc,
            SubnetSelection = new SubnetSelection
            {
                SubnetType = SubnetType.PUBLIC
            }
        });
        database.Connections.AllowDefaultPortFrom(bastion);

        var rdsSecurityGroup = SecurityGroup.FromSecurityGroupId(this, $"{prefix}-rds-security-group", securityGroupId);
        rdsSecurityGroup.AddIngressRule(Peer.Ipv4(cidr), Port.Tcp(5432));
    }
}
