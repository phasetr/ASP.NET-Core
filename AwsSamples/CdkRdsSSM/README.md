# Welcome to your CDK C# project!

- AWS公式：[AWS Systems Manager Session Managerでポートフォワーディングを使用してリモートホストに接続する](https://aws.amazon.com/jp/blogs/news/use-port-forwarding-in-aws-systems-manager-session-manager-to-connect-to-remote-hosts-jp/)
- [AWS CDKでAWS App Runner #devio2022](https://www.youtube.com/watch?v=SeII-sCqkts)
  - [GitHub](https://github.com/yamatatsu/slide-devio-2022)
- [[AWS CDK] Aurora Serverless v2をL2 Constructで定義できるようになりました](https://dev.classmethod.jp/articles/aws-cdk-aurora-serverless-v2/)
- 利用上の注意
  - データベースのID・パスワードは`Secrets Manager`に格納されている。
  - `Bastion`による`Session Manager`も導入済み。利用法は[CLI Session Managerでのポートフォワーディング, リモートホストへの接続](https://phasetr.com/archive/fc/pg/aws/#cli-session-manager)参照。

## データベース接続手順

- `CDK`で環境構築する

```shell
export AWS_PROFILE=dev
aws whoami
```

```shell
cdk deploy --profile dev
```

- `Systems Manager`の`Fleet Manager`、`RDS`のエンドポイントで次の値を調べておく
  - `cdk deploy`すると`SSM_INSTANCE_ID`は最後に`Outputs`として表示される

```shell
SSM_INSTANCE_ID="i-0e5ac0081e9d266b4"
HOST="cdk-rds-ssm-db-cluster.cluster-cbiybupppygm.ap-northeast-1.rds.amazonaws.com"
```

- 次のコマンドで`SSM`に接続確認する：接続に成功したら即閉じて良い

```shell
aws ssm start-session --target ${SSM_INSTANCE_ID}
```

- PostgreSQLサーバーのリモートポートへのコネクション転送セッションを開く

```shell
# aws ssm start-session --target <ssm-managed-instance-id> --document-name AWS-StartPortForwardingSessionToRemoteHost --parameters '{"portNumber":["3306"],"localPortNumber":["1053"],"host":["remote-database-host-name"]}'
aws ssm start-session --target ${SSM_INSTANCE_ID} \
   --document-name AWS-StartPortForwardingSessionToRemoteHost \
   --parameters '{"portNumber":["5432"],"localPortNumber":["1053"],"host":["cdk-rds-ssm-db-cluster.cluster-cbiybupppygm.ap-northeast-1.rds.amazonaws.com"]}'
````

- コネクションが転送されているか確認：`A5M2`などで接続確認すればよい
