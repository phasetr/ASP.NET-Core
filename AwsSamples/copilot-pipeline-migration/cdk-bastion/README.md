# Welcome to your CDK C# project!

- AWS公式：[AWS Systems Manager Session Managerでポートフォワーディングを使用してリモートホストに接続する](https://aws.amazon.com/jp/blogs/news/use-port-forwarding-in-aws-systems-manager-session-manager-to-connect-to-remote-hosts-jp/)
- `copilot`で作った`RDS`に`SSM`で接続するための`CDK`。
- [参考](https://aws.amazon.com/jp/blogs/news/use-port-forwarding-in-aws-systems-manager-session-manager-to-connect-to-remote-hosts-jp/)
- 接続確認：`ssm-managed-instance-id`は適切に調べる

## 手順

### ステージング

- `Systems Manager`の`Fleet Manager`、`RDS`のエンドポイントで次の値を調べておく

```shell
SSM_INSTANCE_ID="i-0826c84df0838ff33"
HOST="pipeline-migration-stg-pi-pipelinemigrationcluster-iqfagvgqttup.cluster-cbiybupppygm.ap-northeast-1.rds.amazonaws.com"
```

- 必要に応じて基本的な接続確認

```shell
aws ssm start-session --target ${SSM_INSTANCE_ID}
```

- PostgreSQLサーバーのリモートポートへのコネクション転送セッションを開く

```shell
# aws ssm start-session --target <ssm-managed-instance-id> --document-name AWS-StartPortForwardingSessionToRemoteHost --parameters '{"portNumber":["3306"],"localPortNumber":["1053"],"host":["remote-database-host-name"]}'
aws ssm start-session --target ${SSM_INSTANCE_ID} \
   --region ap-northeast-1 \
   --document-name AWS-StartPortForwardingSessionToRemoteHost \
   --parameters '{"portNumber":["5432"],"localPortNumber":["1053"],"host":["pipeline-migration-stg-pi-pipelinemigrationcluster-iqfagvgqttup.cluster-cbiybupppygm.ap-northeast-1.rds.amazonaws.com"]}'
````

- コネクションが転送されているか確認

```shell
psql -h 127.0.0.1 --port 1053 -U postgres -W
```

### プロダクション

- `Systems Manager`の`Fleet Manager`、`RDS`のエンドポイントで次の値を調べておく

```shell
SSM_INSTANCE_ID="i-0979243ab5b3fb578"
HOST="pipeline-migration-prod-p-pipelinemigrationcluster-sdzswufc5yux.cluster-cbiybupppygm.ap-northeast-1.rds.amazonaws.com"
```

- 必要に応じて基本的な接続確認

```shell
aws ssm start-session --target ${SSM_INSTANCE_ID}
```

- PostgreSQLサーバーのリモートポートへのコネクション転送セッションを開く

```shell
aws ssm start-session --target ${SSM_INSTANCE_ID} \
   --region ap-northeast-1 \
   --document-name AWS-StartPortForwardingSessionToRemoteHost \
   --parameters '{"portNumber":["5432"],"localPortNumber":["1053"],"host":["pipeline-migration-prod-p-pipelinemigrationcluster-sdzswufc5yux.cluster-cbiybupppygm.ap-northeast-1.rds.amazonaws.com"]}'
````

- コネクションが転送されているか確認

```shell
psql -h 127.0.0.1 --port 1053 -U postgres -W
```
