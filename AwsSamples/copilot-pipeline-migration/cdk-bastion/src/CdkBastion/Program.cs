using Amazon.CDK;
using CdkBastion;

var app = new App();
var unused1 = new CdkBastionStack(app, "copilot-pipeline-migration-cdk-bastion-stack-stg", new MyStackProps
{
    Configuration = new MyConfiguration
    {
        EnvironmentName = "stg",
        // DB ClusterのARNを指定：RDSの「DBクラスター」で「リージョン別クラスター＞設定タブ」を調べる
        DbClusterArn = "arn:aws:rds:ap-northeast-1:573143736992:cluster:pipeline-migration-stg-pi-pipelinemigrationcluster-iqfagvgqttup",
        // ステージング環境のVPC IDを指定
        VpcId = "vpc-01cbf91af06f9fbed",
        // VPCのセキュリティグループから：対応するセキュリティグループ名は`pipeline-migration-stg-pipeline-migration-svc-AddonsStack-Q52UMXGQ2NST-pipelinemigrationclusterDBClusterSecurityGroup-1QSRTSS7Q4NW0`
        SecurityGroupId = "sg-082487acafe8ce504",
        // VPCのCIDRを指定
        Cidr = "10.0.0.0/24"
    }
});
// var unused2 = new CdkBastionStack(app, "copilot-pipeline-migration-cdk-bastion-stack-prod", new MyStackProps
// {
//     Configuration = new MyConfiguration
//     {
//         EnvironmentName = "prod",
//         // DB ClusterのARNを指定：RDSの「DBクラスター」で「リージョン別クラスター＞設定タブ」を調べる
//         DbClusterArn = "arn:aws:rds:ap-northeast-1:573143736992:cluster:pipeline-migration-prod-p-pipelinemigrationcluster-sdzswufc5yux",
//         // VPC IDを指定
//         VpcId = "vpc-0e9af8fedfad9f1dc",
//         // VPCのセキュリティグループから：対応するグループ名は`pipeline-migration-prod-pipeline-migration-svc-AddonsStack-1HHWX18ODON4O-pipelinemigrationclusterDBClusterSecurityGroup-752NGKXRQERP`
//         SecurityGroupId = "sg-0f8ac7f69f3565ff5",
//         // VPCのCIDRを指定
//         Cidr = "10.0.0.0/24"
//     }
// });
app.Synth();
