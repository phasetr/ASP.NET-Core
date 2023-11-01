# Welcome to your CDK C# project!

- オリジナル`CdkUrlShortener`を修正して複数の`Lambda`を追加
- 参考：[aws-cdk-build-package-publish-dotnet-lambda-function](https://github.com/aws-samples/aws-cdk-build-package-publish-dotnet-lambda-function/tree/main)
- エンドポイントのURL例：リージョンによっても変わるので注意
  - <https://xxxyyyzzz.execute-api.us-east-1.amazonaws.com/prod/>
  - <https://xxxyyyzzz.execute-api.us-east-1.amazonaws.com/prod/>

## 実行サンプル

- `HOST`は`cdk deploy`の結果を見て適切に書き換えること

```shell
HOST=26reka5yzj
curl https://${HOST}.execute-api.ap-northeast-1.amazonaws.com/prod/
curl https://${HOST}.execute-api.ap-northeast-1.amazonaws.com/prod/Calculator/add/1/2
curl https://${HOST}.execute-api.ap-northeast-1.amazonaws.com/prod/function-two
curl https://${HOST}.execute-api.ap-northeast-1.amazonaws.com/prod/function-two/Calculator/add/1/2
curl https://${HOST}.execute-api.ap-northeast-1.amazonaws.com/prod/function-three
curl https://${HOST}.execute-api.ap-northeast-1.amazonaws.com/prod/function-three/Calculator/add/1/2
```

## memo

### Init project

- 初期化してソリューション化

```shell
mkdir CdkLambdaByCsharp
cd $_
cdk init app --language=csharp
dotnet new sln --name CdkLambdaByCsharp
dotnet sln add CdkLambdaByCsharp.sln src/CdkLambdaByCsharp/CdkLambdaByCsharp.csproj
```

- プロジェクトの追加

```shell
mkdir lambda
cd $_
dotnet new serverless.AspNetCoreMinimalAPI -o ./ --name FunctionOne
cd ..
dotnet sln add lambda/src/FunctionOne/FunctionOne.csproj
```

- `CDK`のプロジェクトに適切に追記
- `Docker`立ち上げ
- `CDK`でデプロイ

```shell
# 必要に応じて`export AWS_DEFAULT_PROFILE=<USER_NAME>`でユーザー切り替え
cdk deploy --profile dev
```

- もう一つの`Lambda`を追加

```shell
# cd lambda
dotnet new serverless.AspNetCoreMinimalAPI -o ./ --name FunctionTwo
cd ..
dotnet sln add lambda/src/FunctionTwo/FunctionTwo.csproj
```

- 追加した`Lambda`用の`Program.cs`を次のように修正

```csharp
// app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda - Function Two!");
// => 以下のように修正
app.UsePathBase(new PathString("/functiontwo"));
app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda - Function Two!");
app.UseRouting();
```

- 直上の手順で設定したパスに合わせて`CDK`の`Stack`に追記
- `CDK`のプロジェクトに適切に追記
- `CDK`でデプロイ

```shell
# 必要に応じて`export AWS_DEFAULT_PROFILE=<USER_NAME>`でユーザー切り替え
cdk deploy --profile dev
```

## ORIG

This is a blank project for CDK development with C#.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

### Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template
