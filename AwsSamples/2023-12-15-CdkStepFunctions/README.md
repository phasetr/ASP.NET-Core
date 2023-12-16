# README

- AWS公式の単純なサンプル：[LambdaStep Functions使用するためのステートマシンの作成 AWS CDK](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/tutorial-lambda-state-machine-cdk.html)
- [[AWS CDK] Step Functions の Lambda タスクを同期呼び出しする場合の ASL の書き方について紹介・比較する](https://zenn.dev/hassaku63/articles/aefff9ebfee49f)
    - [GitHub](https://github.com/hassaku63/cdk-sfn-example)  
- [AWS Step FunctionsとLambdaを使ってじゃんけんのフローを作ってみた](https://liginc.co.jp/592766)

## `Lambda` `Step Functions`使用するためのステートマシンの作成`AWS CDK`

- サンプルの`Step Functions`実行

```shell
stackName=cdk-step-functions-stack \
  && runName=$(openssl rand -base64 100 | tr -dc 'a-zA-Z' | fold -w 10 | head -n 1) \
  && arn=$(aws cloudformation describe-stacks --stack-name ${stackName} --query 'Stacks[].Outputs[?OutputKey==`cdkstepfunctionsstatemachine1arn`].OutputValue' --output text --profile dev) \
  && aws stepfunctions start-execution \
    --state-machine-arn ${arn} \
    --name ${runName} \
    --input '{"name":"World"}' \
  && echo runName: ${runName}
```

## AWS Step FunctionsとLambdaを使ってじゃんけんのフローを作ってみた

```shell
stackName=cdk-step-functions-stack \
 && runName=$(openssl rand -base64 100 | tr -dc 'a-zA-Z' | fold -w 10 | head -n 1) \
 && arn=$(aws cloudformation describe-stacks --stack-name ${stackName} --query 'Stacks[].Outputs[?OutputKey==`cdkstepfunctionsstatemachine2arn`].OutputValue' --output text --profile dev) \
 && aws stepfunctions start-execution \
   --state-machine-arn ${arn} \
   --name ${runName} \
   --input '{"throw": "1"}' \
 && echo runName: ${runName}
```
