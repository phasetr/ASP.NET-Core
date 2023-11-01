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

### デプロイ・確認

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

### 削除

```shell
cdk destroy --profile dev
```
