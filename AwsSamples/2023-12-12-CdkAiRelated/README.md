# Welcome to your CDK C# project!

- [Amazon BedrockのClaudeとAmazon Connectを利用し、電話で色々な質問に答えてくれるコールセンター向けAIチャットボットを構築してみた](https://dev.classmethod.jp/articles/amazon-bedrock-claude-connect-lex/)
- [[Amazon Bedrock] Lambda関数からBedrockを呼び出してみた](https://dev.classmethod.jp/articles/invoke-bedrock-form-lambda-function/)
- 参考：[aws-cdk-build-package-publish-dotnet-lambda-function](https://github.com/aws-samples/aws-cdk-build-package-publish-dotnet-lambda-function/tree/main)

## CDK

### デプロイと実行確認

```shell
cdk deploy --profile dev --require-approval never
```

- `API`側の確認

```shell
export API_GATEWAY_URL=$(aws cloudformation describe-stacks --stack-name cdk-ai-related-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdkairelatedstackapigwurldev`].OutputValue' --output text --profile dev) \
  && echo ${API_GATEWAY_URL} \
  && curl -s ${API_GATEWAY_URL} \
  && curl -s ${API_GATEWAY_URL}one \
  && curl -s ${API_GATEWAY_URL}calc/add/1/2 \
  && curl -s ${API_GATEWAY_URL}weather
```

### 環境削除

```shell
cdk destroy --profile dev
```

## LambdaBedrock

- `ASP.NET Core`による`Lambda`の`API`アプリ
- `ASP.NET Core`は`Lambda`だからと言って特別な処理・対処が必要なわけではない

## OpenAiApp

- コンソールアプリ
- （`Rider`の）起動構成で`OPENAI_API_KEY`を設定する
