using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Constructs;

namespace CiCdVpcEcs;

public class CiCdVpcEcsStack : Stack
{
    internal CiCdVpcEcsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        const string microServiceName = "cdk-official-ecs-etc-csharp";
        const string vpcNonProdId = $"{microServiceName}-ci-cd-vpc-nonprod-id";
        const string vpcProdId = $"{microServiceName}-ci-cd-vpc-prod-id";
        const string ecsNonProdId = $"{microServiceName}-ci-cd-ecs-nonprod";
        const string ecsProdId = $"{microServiceName}-ci-cd-ecs-prod";

        var vpcNonProd = new Vpc(this, vpcNonProdId, new VpcProps
        {
            MaxAzs = 3
        });
        var unused1 = new Cluster(this, ecsNonProdId, new ClusterProps
        {
            Vpc = vpcNonProd,
            ClusterName = ecsNonProdId
        });
        /*
        var unused2 = new ApplicationLoadBalancedFargateService(this, "MyFargateServiceNonProd",
            new ApplicationLoadBalancedFargateServiceProps
            {
                Cluster = unused1, // Required
                DesiredCount = 1, // Default is 1
                TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                {
                    Image = ContainerImage.FromRegistry("amazon/amazon-ecs-sample")
                },
                MemoryLimitMiB = 2048, // Default is 256
                PublicLoadBalancer = true // Default is true
            }
        );
        */
        
        var vpcProd = new Vpc(this, vpcProdId, new VpcProps
        {
            MaxAzs = 3
        });
        var unused3 = new Cluster(this, ecsProdId, new ClusterProps
        {
            Vpc = vpcProd,
            ClusterName = ecsProdId
        });
        // var unused4 = new ApplicationLoadBalancedFargateService(this, "MyFargateServiceProd",
        //     new ApplicationLoadBalancedFargateServiceProps
        //     {
        //         Cluster = unused3, // Required
        //         DesiredCount = 1, // Default is 1
        //         TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
        //         {
        //             Image = ContainerImage.FromRegistry("amazon/amazon-ecs-sample")
        //         },
        //         MemoryLimitMiB = 2048, // Default is 256
        //         PublicLoadBalancer = true // Default is true
        //     }
        // );
    }
}
