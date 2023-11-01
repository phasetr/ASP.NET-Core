# Welcome to your CDK C# project!

## memo

- S3の設定の参考
  - [AWS CDK v2でOACってどうやってやるの？CloudFront+S3+OAC構築（コード付き-Typescript）](https://qiita.com/ta__k0/items/bd700a074c394aa4d6f4)
- [class CfnDistribution (construct)](https://docs.aws.amazon.com/cdk/api/v2/docs/aws-cdk-lib.aws_cloudfront.CfnDistribution.html)
- [interface DistributionConfigProperty · AWS CDK](https://docs.aws.amazon.com/cdk/api/v2/docs/aws-cdk-lib.aws_cloudfront.CfnDistribution.DistributionConfigProperty.html#origins)
- [interface OriginProperty · AWS CDK](https://docs.aws.amazon.com/cdk/api/v2/docs/aws-cdk-lib.aws_cloudfront.CfnDistribution.OriginProperty.html#originaccesscontrolid)

## 実行

```shell
cdk deploy --profile dev
```

## 削除

- `S3 Stack`の名前を記録する

```shell
S3StackName="cdks3cloudfrontstack-bucket83908e77-12vq6kqjxjr2w"
aws s3 rm s3://${S3StackName} --recursive --profile dev
cdk destroy --profile dev
```

# ORIG

This is a blank project for CDK development with C#.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template
