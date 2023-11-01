# Welcome to your CDK C# project!

`CDK`は`C#`で`API Gateway+Lambda+DynamoDB`を構成し,
`Lambda`は`JavaScript`.
`Pipeline`追加版.

## original

You should explore the contents of this project. It demonstrates a CDK app with an instance of a stack (`WorkshopStack`)
which contains an Amazon SQS queue that is subscribed to an Amazon SNS topic.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET Core CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk ls`           list all stacks in the app
* `cdk synth`       emits the synthesized CloudFormation template
* `cdk deploy`      deploy this stack to your default AWS account/region
* `cdk diff`        compare deployed stack with current state
* `cdk docs`        open CDK documentation

Enjoy!

## MEMO

- [.NET Workshop](https://cdkworkshop.com/ja/40-dotnet.html)
- [CodeBuildで使えるランタイムと対応](https://docs.aws.amazon.com/codebuild/latest/userguide/available-runtimes.html)
  - [【AWS CDK Workshop】CDK PipelinesのセクションをCodeCommitではなくGitHubでやってみた](https://qiita.com/shimi7o/items/cf69aac8e4b5f2f1dd52)
- TODO: PipelineをCodeCommitからGitHubに移行: [参考](https://qiita.com/shimi7o/items/cf69aac8e4b5f2f1dd52)
- [Another sample: .NETなCDKで.NETなLambdaを自動デプロイしていく](https://buildersbox.corp-sansan.com/entry/2021/05/31/110000)
- [API Gateway + Lambda で .NET 6 の Minimal API を実行してみる](https://dev.classmethod.jp/articles/api-gateway-lambda-net-6-minimal-api/)

## Commands

```sh
cdk init sample-app --language csharp
cdk synth # CFnテンプレート出力
cdk bootstrap # ブートストラップスタックを環境にインストール
cdk deploy # デプロイ
# cdk deploy --hotswap # ドリフトが起こるため開発環境以外で使ってはいけない
# cdk watch # 更新監視してくれる
cdk diff # ローカルの設定と公開設定の差分を取る
cdk destroy # 作成した環境の削除
```
