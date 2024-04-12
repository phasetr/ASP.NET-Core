# Welcome to your CDK C# project!

- オリジナル`CdkUrlShortener`を修正して単一の`Lambda`をルート直下に追加
- 参考：[aws-cdk-build-package-publish-dotnet-lambda-function](https://github.com/aws-samples/aws-cdk-build-package-publish-dotnet-lambda-function/tree/main)

## `CDK`

### デプロイと実行確認

```shell
cdk deploy --profile dev --require-approval never
```

- `API`側の確認

```shell
export API_URL=$(aws cloudformation describe-stacks --stack-name ls-dev --query 'Stacks[].Outputs[?OutputKey==`lsapiurldev`].OutputValue' --output text --profile dev) \
  && echo ${API_URL} \
  && curl -s ${API_URL} \
  && curl -s ${API_URL}Calculator/add/1/2
```

```shell
export WEB_URL=$(aws cloudformation describe-stacks --stack-name ls-dev --query 'Stacks[].Outputs[?OutputKey==`lsweburldev`].OutputValue' --output text --profile dev) \
  && echo ${WEB_URL} \
  && curl -s ${WEB_URL}api \
  && curl -s ${WEB_URL}Calculator/add/1/2
```

```shell
export BLAZOR_URL=$(aws cloudformation describe-stacks --stack-name ls-dev --query 'Stacks[].Outputs[?OutputKey==`lsburldev`].OutputValue' --output text --profile dev) \
  && echo ${BLAZOR_URL} \
  && curl -s ${BLAZOR_URL}api \
  && curl -s ${BLAZOR_URL}Calculator/add/1/2
```

### 環境削除

```shell
cdk destroy --profile dev
```

## `Blazor`

- `WebSocket`がうまく行かない？

### ルートパスの設定

- [参考](https://qiita.com/jsakamoto/items/7f1bdf4ce3d9c757ed94)
- 特に[Blazor WebAssembly スタンドアロンの場合](https://qiita.com/jsakamoto/items/7f1bdf4ce3d9c757ed94#blazor-webassembly-スタンドアロンの場合)を参考にすること

```shell
curl https://y08wf42as2.execute-api.ap-northeast-1.amazonaws.com/prod/_blazor
```

```shell
curl -X POST https://y08wf42as2.execute-api.ap-northeast-1.amazonaws.com/prod/_blazor
```

```shell
curl -X POST https://y08wf42as2.execute-api.ap-northeast-1.amazonaws.com/prod/_blazor?id=6lSlCppiW0FdPQgeCJa4nw 
```
