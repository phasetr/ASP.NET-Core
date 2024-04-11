# Welcome to your CDK C# project!

- オリジナル`CdkUrlShortener`を修正して単一の`Lambda`をルート直下に追加
- 参考：[aws-cdk-build-package-publish-dotnet-lambda-function](https://github.com/aws-samples/aws-cdk-build-package-publish-dotnet-lambda-function/tree/main)

## デプロイと実行確認

```shell
cdk deploy --profile dev --require-approval never
```

- `API`側の確認

```shell
export API_GATEWAY_URL=$(aws cloudformation describe-stacks --stack-name ls-dev --query 'Stacks[].Outputs[?OutputKey==`lsapigwurldev`].OutputValue' --output text --profile dev) \
  && echo ${API_GATEWAY_URL} \
  && curl -s ${API_GATEWAY_URL} \
  && curl -s ${API_GATEWAY_URL}Calculator/add/1/2
```

## 環境削除

```shell
cdk destroy --profile dev
```
