# Welcome to your CDK C# project!

CDKはC#でAPI Gateway+Lambda+DynamoDBを構成し、
LambdaもC#。

## original

This is a blank project for CDK development with C#.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template

## memo

- [Another sample: .NETなCDKで.NETなLambdaを自動デプロイしていく](https://buildersbox.corp-sansan.com/entry/2021/05/31/110000)
- [CDK API Reference](https://docs.aws.amazon.com/cdk/api/latest/docs/aws-construct-library.html)

### command

```shell
cdk init app --language csharp
cdk bootstrap
cdk deploy
mkdir lambda && cd lambda
dotnet new install Amazon.Lambda.Templates
dotnet new lambda.EmptyFunction --name HelloHandler --profile default --region ap-northeast-1
dotnet add package Amazon.Lambda.APIGatewayEvents

dotnet tool install -g Amazon.Lambda.Tools
dotnet tool update -g Amazon.Lambda.Tools

dotnet lambda deploy-function HelloHandler --function-role role
```
