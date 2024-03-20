# README

- [`.NET Core`アプリを`AWS Lambda`で動かす](https://qiita.com/mahiya/items/d724a5831a7fceb72f2b)

```shell
dotnet new install Amazon.Lambda.Templates
dotnet new lambda --list
```

## `.NET`コマンドで雛形作成

- テンプレートからプロジェクトを作成する

```shell
dotnet new serverless.AspNetCoreWebApp --name LambdaRazorPages --region ap-northeast-1 --profile dev
```

## デプロイ

- 必要に応じて次のコマンドをグローバルにインストールする

```shell
dotnet tool install -g Amazon.Lambda.Tools
```

- ソリューションルートで次のコマンドを実行

```shell
dotnet lambda deploy-function \
  -pl src/LambdaRazorPages \
  -frun dotnet8 \
  -fn LambdaRazorPages \
  -fms 1024 \
  -ft 300
```

- `S3`バケットを作成する

```shell
bucketName=pt-lambda-razor-pages-bucket && stackName=LambdaRazorPagesStack
```

```shell
aws s3 mb s3://${bucketName} --region ap-northeast-1 --profile dev
```

```shell
dotnet lambda deploy-serverless \
  --profile dev \
  --region ap-northeast-1 \
  -pl src/LambdaRazorPages \
  -sn ${stackName} \
  -sb ${bucketName}
```

- `Lambda`関数を削除する

```shell
aws cloudformation delete-stack --stack-name ${stackName} --profile dev
```

- `S3`バケットを削除する

```shell
aws s3 rb s3://${bucketName} --force
```
