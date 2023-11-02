# README

## メモ

- TODO: `SAM`連携開発
- 参考：[CloudFront と API Gateway で SPA の CORS 問題をイイ感じに解決する](https://dev.classmethod.jp/articles/spa-cloudfront-and-api-gateway-voiding-cors/)
  - これは難しそう：頑張って`CORS`対応しよう
  - 参考：CORS対策のフロント・バックエンドの統合
    - <https://dev.to/evnz/single-cloudfront-distribution-for-s3-web-app-and-api-gateway-15c3>
    - GitHub: <https://github.com/evnz/blog-example-single-cf-distribution/tree/main>
- 参考・後で実装：[dynamodbのローカルでのテスト並列実行](https://dev.classmethod.jp/articles/localstack-dynamodb-concurrency/)
- 初め`OAC`でエラーが起きていたため`AWS`に質問を投げた時のメモ
  - `OAC`のエラーは`CloudTrail`のログを見るとわかる。
  - `CloudFormation`含めてログをしっかり見る。
  - 今回は`"errorMessage": "The parameter Name is too big."`だった
  - `OAC`は`Name`は最大64文字まで

## ローカル開発時の`docker`

- `Blazor`・`ServerlessApi`はそれぞれコマンドラインから実行する。
  （`docker compose`で立ち上げるわけではない。）
- 設定はルート直下の`compose.yml`参照。
- `Blazor`は<http://localhost>に、
  `ServerlessApi`は<http://localhost/api>に転送している。
- ポート指定は`Blazor`, `ServerlessApi`それぞれの設定から適切に抜き出すこと。

## `AWS`

### `SAM`

- `TODO`: 適切な形で`CDK`から`SAM`のテンプレートを書き出す
- テンプレートを書き出したら適切な`SAM`のコマンドを使って各種処理を進める
- [コマンド集の参考ページ](https://zenn.dev/sugikeitter/articles/sam-written-by-cdk)

#### `SAM CLI` with `CDK

- 利用イメージ：`CDK`は`AWS`リソースを構築するためだけのツールであるため,
  `CDK`がカバーしない・サーバーレスアプリケーションの「ローカルテスト」の機能を`SAM`で補完する
- [Qiita: AWS SAM with CDK](https://qiita.com/nisim/items/b4aaaef4da116f381f51)

> さていよいよ本題です。
> `CDK`で作成したスタックを`SAM CLI`の`local`テストでテストします。
> 具体的には`sam local invoke`コマンドの`-t`オプションの引数に`cdk synth`などで生成されるテンプレート`(cdk.out/hogehogeStack.template.json)`を指定し、
> `cdk`で作成した`yaml`ファイルを`sam`の`cli`でテストします。

```shell
sam local invoke -e events/event.json -t ./cdk.out/cdk-sam-blazor-aspnet-core-dynamo-db-stack-dev.template.json CdkFunction
```

#### コマンド集

- キャッシュを利用したビルド、コンテナランタイムがあれば Lambda 関数の言語ランタイムを気にせずビルドできる。

```shell
sam build --cached --use-container
```

- Lambda 関数にイベント引数を渡してローカル実行。

```shell
sam local invoke --debug -e event.json [FUNCTION_LOGICAL_ID]
```

- `API Gateway`のエンドポイントをローカルで起動、ポート番号も選べる。

```shell
sam local start-api --debug -p 3000
```

- `SAM`で定義した`Lambda`関数を別のコードのテストのため呼び出したい場合などに役立つ。
  参考：[sam local start-lambda](https://docs.aws.amazon.com/ja_jp/serverless-application-model/latest/developerguide/sam-cli-command-reference-sam-local-start-lambda.html)

```shell
sam local start-lambda && aws lambda invoke --function-name "[FUNCTION_NAME]" --endpoint-url "http://127.0.0.1:3001" --no-verify-ssl out.txt
``` 

- 変更セットの実行はせず、変更内容の確認のみ。

```shell
sam deploy --no-execute-changeset
````

- SAM 以外のリソースも変更セットを作成せずにそのままデプロイできるので、開発環境で手早くデプロイしたい時に便利。

```shell
sam sync --stack-name [STACK_NAME]
```

### `CDK`

#### デプロイ・確認

```shell
cdk deploy --profile dev
```

- メモ：スタック取得用コマンド

```shell
aws cloudformation describe-stacks --stack-name cdk-sam-blazor-aspnet-core-dynamo-db-stack-dev
```

- `API`側の確認

```shell
export ApiGwUrl=$(aws cloudformation describe-stacks --stack-name cdk-sam-blazor-aspnet-core-dynamo-db-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdksamblazoraspnetcoredynamodbapigwurldev`].OutputValue' --output text --profile dev)
echo ${ApiGwUrl}
curl -s ${ApiGwUrl}
```

- `Blazor`側の確認

```shell
export S3BucketName=$(aws cloudformation describe-stacks --stack-name cdk-sam-blazor-aspnet-core-dynamo-db-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdksamblazoraspnetcoredynamodbs3bucketnamedev`].OutputValue' --output text --profile dev)
echo ${S3BucketName}
export DomainName=$(aws cloudformation describe-stacks --stack-name cdk-sam-blazor-aspnet-core-dynamo-db-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdksamblazoraspnetcoredynamodbcloudfrontdomainnamedev`].OutputValue' --output text --profile dev)
echo ${DomainName}

dotnet publish Blazor -c Release -o ./publish
aws s3 sync ./publish/wwwroot s3://${S3BucketName} --profile dev

echo ${S3BucketName}
echo https://${DomainName}
```

#### 削除

```shell
cdk destroy --profile dev
```
