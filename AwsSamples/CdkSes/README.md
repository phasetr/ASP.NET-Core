# Welcome to your CDK C# project!

- [Send Emails from ASP.NET Core using Amazon SES: Complete Guide](https://codewithmukesh.com/blog/send-emails-from-aspnet-core-using-amazon-ses/)
  - [GitHub](https://github.com/iammukeshm/send-emails-from-aspnet-core-using-amazon-ses/tree/main/AmazonSES.Demo)
- [AWS CDKでSPF、DKIM、DMARC設定をしたAmazon SES を作成する](https://qiita.com/watany/items/5700b391f6f5c69e0ae0)
- [【AWS】SES構築について](https://qiita.com/y-okuhira/items/e0a393dc3b6cec653c76)
  - 送信件数制限の解除やbounceなど一通りの確認
- コードサンプル
  - [c#でAWS SESメール送信方法（ドメイン認証編）](https://usefuledge.com/csharp-sendmail-aws-ses.html#%E3%83%89%E3%83%A1%E3%82%A4%E3%83%B3%E8%AA%8D%E8%A8%BC%E3%82%92%E8%A1%8C%E3%81%86)
  - AWS SDKによる実装：[Hello, SES! Getting Started with .NET on AWS: Amazon Simple Email Service](https://davidpallmann.hashnode.dev/hello-ses#heading-step-2-obtain-access-keys)

## 重要な注意

- サンドボックス環境のままではコンソールで設定したアドレスにしかメールが送れないため、
  なるべく早い段階でサンドボックスの制限を外すこと。
- `WebApi`の`/mail`にメール送信を設定してある。
- `WebApi`の`/sdkmail`にAWS SDKによるメール送信を設定してある。
- 両者で参照するべきAWS情報が違うため注意すること。
  - `/mail`は[c#でAWS SESメール送信方法（ドメイン認証編）](https://usefuledge.com/csharp-sendmail-aws-ses.html#%E3%83%89%E3%83%A1%E3%82%A4%E3%83%B3%E8%AA%8D%E8%A8%BC%E3%82%92%E8%A1%8C%E3%81%86)をもとに`SES`から取得する。
  - `/sdkmail`は`IAM`でのアクセスキー・シークレットキーを設定する。

## Orig

This is a blank project for CDK development with C#.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template
