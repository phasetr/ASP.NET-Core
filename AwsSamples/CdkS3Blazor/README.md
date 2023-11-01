# Welcome to your CDK C# project

- `C#`の`Blazor`の（AWSでの）デプロイ方法を確認するため、`S3`と`CloudFront`を立ててデプロイする手順をまとめた。
- `Blazor`は`dotnet new`で生成したそのままの内容。

## memo

### デプロイ・確認

```shell
cdk deploy --profile dev
````

```shell
export S3BucketName=$(aws cloudformation describe-stacks --stack-name cdk-s3-blazor-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdks3blazors3bucketnamedev`].OutputValue' --output text --profile dev) 
export DomainName=$(aws cloudformation describe-stacks --stack-name cdk-s3-blazor-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdks3blazorcloudfrontdomainnamedev`].OutputValue' --output text --profile dev)
dotnet publish BlazorWasm -c Release -o ./publish
aws s3 sync ./publish/wwwroot s3://${S3BucketName} --profile dev
echo https://${DomainName}
```

### 削除

```shell
cdk destroy --profile dev
```
