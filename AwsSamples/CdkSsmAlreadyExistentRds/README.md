# Welcome to your CDK C# project!

## SSM

- 接続確認：`ssm-managed-instance-id`は適切に調べる

```shell
SSM_MANAGED_INSTANCE_ID=i-0ced3753da4212281
# aws ssm start-session --target <ssm-managed-instance-id>
aws ssm start-session --target ${SSM_MANAGED_INSTANCE_ID}
```

- PostgreSQLサーバーのリモートポートへのコネクション転送セッションを開く
- `host`はエンドポイント名を指定すれば良い。

```shell
# aws ssm start-session --target <ssm-managed-instance-id> --document-name AWS-StartPortForwardingSessionToRemoteHost --parameters '{"portNumber":["3306"],"localPortNumber":["1053"],"host":["remote-database-host-name"]}'
aws ssm start-session --target ${SSM_MANAGED_INSTANCE_ID} \
   --document-name AWS-StartPortForwardingSessionToRemoteHost \
   --parameters '{"portNumber":["5432"],"localPortNumber":["1053"],"host":["webapi-staging-ap-addonsstack-1-apclusterdbcluster-qpodz4ycshv3.cluster-cbiybupppygm.ap-northeast-1.rds.amazonaws.com"]}'
```

- コネクションが転送されているか確認

```shell
psql -h 127.0.0.1 --port 1053 -U postgres -W
```

## ORIG
You should explore the contents of this project. It demonstrates a CDK app with an instance of a stack (`CdkSsmAlreadyExistentRdsStack`)
which contains an Amazon SQS queue that is subscribed to an Amazon SNS topic.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk ls`           list all stacks in the app
* `cdk synth`       emits the synthesized CloudFormation template
* `cdk deploy`      deploy this stack to your default AWS account/region
* `cdk diff`        compare deployed stack with current state
* `cdk docs`        open CDK documentation

Enjoy!

## copilot

- `copilot`ディレクトリを削除する.
- 必要に応じて`Route 53`を含めドメイン関係の設定をしておく.

```shell
# プロジェクトのトップディレクトリに移動
# プロジェクトルートにいるか確認
export AWS_PROFILE=dev
# rm -rf copilot/*
copilot app init webapi
# 最初からドメイン設定したいなら次のコマンド
# copilot app init webapi --domain math-accessory.com
```

- `env`を作る

```shell
export AWS_PROFILE=dev
copilot env init --name staging \
  --profile dev \
  --app webapi \
  --default-config
```

- `svc`を作る

```shell
export AWS_PROFILE=dev
copilot svc init --name ap \
  --svc-type "Load Balanced Web Service" \
  --dockerfile ./WebApi/WebApi/Dockerfile \
  --port 80
```

- `storage`を作る

```shell
export AWS_PROFILE=dev
copilot storage init -n ap-cluster \
  -l workload \
  -t Aurora \
  -w ap \
  --engine PostgreSQL \
  --initial-db mydb
```

- `git`で差分を見て適切に修正する
- `Dockerfile`で`bundle`の部分をコメントアウトする

```shell
export AWS_PROFILE=dev
copilot env deploy
```

```shell
export AWS_PROFILE=dev
copilot svc deploy
```

```shell
export AWS_PROFILE=dev
copilot svc show
```

- データベースが生成されたか確認する
- `Migrations`にマイグレーション用のファイルがあるか確認する
- `Dockerfile`で`bundle`の部分をコメントアウトを外す

```shell
export AWS_PROFILE=dev
copilot job init -n dbmigrate \
  --dockerfile ./WebApi/WebApi/Dockerfile
```

- `job`の`yml`のデータベースの接続文字列関係の設定を書き換える：特に`secrets`を設定する。

```yaml
secrets:
  APCLUSTER_SECRET: "arn:aws:secretsmanager:hogefuga"
```

- (これ必要？)`IAM`の`yml`でデータベースの接続文字列関係の設定を書き換える

```shell
export AWS_PROFILE=dev
copilot job deploy --name dbmigrate --env staging
copilot job run --name dbmigrate --env staging
```

- `job`のスケジュールを"none"に修正

```yaml
on:
  schedule: "none"
```

- `job`の実行確認

```shell
export AWS_PROFILE=dev
copilot job logs --name dbmigrate --env staging
```

- サービスをデプロイする

```shell
export AWS_PROFILE=dev
copilot svc deploy
```

- `staging`へのマイグレーションを確認する
- `staging`にアクセスして稼働を確認する

```shell
HOST=webap-Publi-156A1UGGWBKNZ-768659207.ap-northeast-1.elb.amazonaws.com
curl -X GET http://${HOST}/
curl -X GET http://${HOST}/api/ApplicationUser/
curl -X GET http://${HOST}/api/ApplicationUser/admin
echo "http://${HOST}/api/ApplicationUser/admin"
```

## `Load Balanced`の場合の手動マイグレーション

```shell
export AWS_PROFILE=dev
copilot svc exec
./bundle
```

## 削除時の手順

```shell
export AWS_PROFILE=dev
copilot job delete --yes
copilot svc delete --yes
copilot env delete --yes
copilot app delete --yes
```
