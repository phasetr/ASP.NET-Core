using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Constructs;

namespace CdkSsmAlreadyExistentRds;

public class CdkSsmAlreadyExistentRdsStack : Stack
{
    private const string Prefix = "cdk-ssm-already-existent-rds";

    // 都度調べて設定する
    private const string VpcId = "vpc-0c4daf635a2f1a23f";
    private const string DataBaseClusterId = "webapi-staging-ap-addonsstack-1-apclusterdbcluster-qpodz4ycshv3";

    internal CdkSsmAlreadyExistentRdsStack(Construct scope, string id, IStackProps props = null) : base(scope, id,
        props)
    {
        var vpc = Vpc.FromLookup(this, VpcId, new VpcLookupOptions
        {
            VpcId = VpcId
        });
        var dbCluster = DatabaseCluster.FromDatabaseClusterAttributes(this, DataBaseClusterId,
            new DatabaseClusterAttributes
            {
                ClusterIdentifier = DataBaseClusterId,
                Port = 5432
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
        dbCluster.Connections.AllowDefaultPortFrom(bastion);

        var _ = new CfnOutput(this, $"{Prefix}-bastion-host-instance-id", new CfnOutputProps
        {
            Value = bastion.InstanceId
        });
    }
}
