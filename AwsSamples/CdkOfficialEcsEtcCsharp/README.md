# Welcome to your CDK C# project!

## Original

- [AWS CDK を使用して、マイクロサービス用の CI/CD パイプラインと Amazon ECS クラスターを自動的に構築します](https://docs.aws.amazon.com/ja_jp/prescriptive-guidance/latest/patterns/automatically-build-ci-cd-pipelines-and-amazon-ecs-clusters-for-microservices-using-aws-cdk.html)
  - 単純なサンプル：[AWS CDKを使用してAWS Fargateサービスを作成する](https://docs.aws.amazon.com/ja_jp/cdk/v2/guide/ecs_example.html)
- [AWS SDK for .NET Version 3 API Reference](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html)
- [API Reference: TypeScript](https://docs.aws.amazon.com/cdk/api/v2/docs/aws-construct-library.html)

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template

## 手順

```shell
cd CiCdVpcEcs/src
cdk deploy
```

- `PipelineStack.cs`で`VPC`とセキュリティグループのIDを指定する
  - セキュリティグループは`EC2`のコンソールで`VPC ID`から調べる
- `S3`に`s3BucketForCode`で設定されている名前のバケットを作る
- `S3`に新規作成したバケットに`starter-code`ディレクトリを`zip`化したファイルをアップロードする

```
cd <プロジェクトルート>
cdk deploy
```
