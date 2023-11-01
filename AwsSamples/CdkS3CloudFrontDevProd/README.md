# Welcome to your CDK C# project!

開発・本番環境を一気に作るサンプル。
特に`S3`と`CloudFront`で`dev`と`prod`の二環境分を作る。

```shell
cdk deploy --all
```

## 削除

```shell
cdk destroy --all --require-approval never
```

## 参考

- [【AWS】Configで分けるCDK環境の紹介【CDK】](https://note.com/asahi_ictrad/n/nf10a4c89c1bf)

## ORIG

It uses the [.NET CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

### Useful commands

* `dotnet build src` compile this app
* `cdk ls`           list all stacks in the app
* `cdk synth`       emits the synthesized CloudFormation template
* `cdk deploy`      deploy this stack to your default AWS account/region
* `cdk diff`        compare deployed stack with current state
* `cdk docs`        open CDK documentation

Enjoy!
