# README

- [公式デベロッパーガイド](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/welcome.html)
- [ステートマシンをローカルでテストする](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/sfn-local.html)
    - `Docker`を使う
    - 参考：[AWS Step Functions Localでモックテストをする](https://qiita.com/taskforce_Hisui/items/397ed38f0cdae94b0941)
- AWS公式の単純なサンプル：[LambdaStep Functions使用するためのステートマシンの作成 AWS CDK](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/tutorial-lambda-state-machine-cdk.html)
- [[AWS CDK] Step Functions の Lambda タスクを同期呼び出しする場合の ASL の書き方について紹介・比較する](https://zenn.dev/hassaku63/articles/aefff9ebfee49f)
    - [GitHub](https://github.com/hassaku63/cdk-sfn-example)  
- [AWS Step FunctionsとLambdaを使ってじゃんけんのフローを作ってみた](https://liginc.co.jp/592766)
- `CDK`で`Cloud Formation`を経由せず直接AWSリソースを更新するコマンド

```shell
cdk deploy --require-approval never --hotswap
```

## 公式チュートリアル

### チュートリアル6まで

- [AWS Step Functions の開始方法](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/getting-started-with-sfn.html)
- チュートリアル4では`Parallel State`のペイロードを固定している
- コンソールから次の入力でステートマシンを実行する

```json
{
  "email": "janedoe@example.com ",
  "ssn": "012-00-0000"
}
```

- `Wait for human approval`を進めるには、メールで送られてきたタスクトークンをコマンドで送信する

```shell
taskToken=<トークンを入力>
aws stepfunctions send-task-success --task-token ${taskToken} --task-output "{}"
```

### チュートリアル7-8

- 入力は次の通り

```shell
{
  "data": {
    "firstname": "Jane",
    "lastname": "Doe",
    "identity": {
      "email": "jdoe@example.com",
      "ssn": "123-45-6789"
    },
    "address": {
      "street": "123 Main St",
      "city": "Columbus",
      "state": "OH",
      "zip": "43219"
    }
  }
}
```

- `Wait for human approval`を進めるには、メールで送られてきたタスクトークンをコマンドで送信する

```shell
taskToken=<トークンを入力>
taskToken=AQB8AAAAKgAAAAMAAAAAAAAAAfVAHVtOHrniTSDmsIthsq+LTSiilxnAgViFdW1eZY6TKJH6MAOrsAaC8FK2SeQkFObRodx/wIdDASsWl5TaD38jEv9pgxPUwjmhSA==Q69lUTsAuBzR6IEKNF44VWHVMog4BSWwGGJ/lUarondBBqjL5Y7+Vh3OKNzbfE/WSrQ3VNJNZDXRIOWR7JUTmGGAJ+1as2rheg/B5HQqn+pMcWhSvpRNNfnUrytEylKjyxuVwSjcQ7AXrj+lOAFHoH40NYIuB+uRvE7fjA/YolKAtG9VNzRvpp/dX2eBFqo/YHfxxv2LbKnZRlRC8CmSs1Neb91wXAdzAwfJ9aqGgPVOJsnAB6mVw2lGZnEtmoYoTEvmBHtAXaaZEr17hIYMw+/NcJspP/bSHjXPkyxNL/LsGzCf2SMRjHjIxnk7/EvjMr8vCHMb9NYg0odg3PDnp30GAikLQv3ze+jnYVCFVUPZy/03AhHldUvzdi5xmjnKybc62KHx6tCLKslNILrECQklX1Fg3ksL2+GAVfkRfnaM5Tj3d0aG62MdwwVaVnSjYAPG04WRNyQBGr+nSMKRW0DqtBa4RZ+nme4rZ4xJNVmpwul1i2lHNVTCcV07dheBdzOeA7xoOpVZ1aHwxxKn
aws stepfunctions send-task-success --task-token ${taskToken} --task-output "{}"
```

### `AWS CLI`で`Step Functions`を実行

## `TODO`: `Step Functions Local`

- TODO：[【Docker】Step Functions localとSAM LocalをDocker内で使用してみる](https://kakkoyakakko2.hatenablog.com/entry/aws-sfn-local)
- [Docker の認証情報と設定](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/sfn-local-config-options.html)

### 公式：ステップ関数をローカル実行

- [ステップ関数をコンピューター上でローカルに実行する](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/sfn-local-computer.html)

```shell
aws stepfunctions --endpoint-url http://localhost:8083 \
  create-state-machine --definition "{\
  \"Comment\": \"A Hello World example of the Amazon States Language using a Pass state\",\
  \"StartAt\": \"HelloWorld\",\
  \"States\": {\
    \"HelloWorld\": {\
      \"Type\": \"Pass\",\
      \"End\": true\
    }\
  }}" --name "HelloWorld" --role-arn "arn:aws:iam::012345678901:role/DummyRole"
```

- ステートマシンの一覧を取得

```shell
aws stepfunctions --endpoint-url http://localhost:8083 list-state-machines --query 'stateMachines[*].{name:name,arn:stateMachineArn}'
```

- ローカル作成したステートマシンを実行

```shell
aws stepfunctions --endpoint-url http://localhost:8083 list-state-machines --query 'stateMachines[*].{name:name,arn:stateMachineArn}' \
  && arn=$(aws stepfunctions --endpoint-url http://localhost:8083 list-state-machines --query 'stateMachines[0].{arn:stateMachineArn}' --output text) \
  && echo ${arn} \
  && executionArn=$(aws stepfunctions --endpoint-url http://localhost:8083 start-execution \
  --state-machine-arn ${arn} \
  --query 'executionArn' --output text) \
  && aws stepfunctions describe-execution --endpoint-url http://localhost:8083 --execution-arn ${executionArn} --output table
```

### 公式：`Step Functions`と`AWS SAM CLI Local`のテスト

- [ステップ関数と AWS SAM CLI ローカルのテスト](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/sfn-local-lambda.html)
- ドキュメントに沿って`SAM`を実行

```shell
sam init --app-template hello-world --runtime python3.11 --name sam-app --no-tracing --no-application-insights --package-type Zip 
```

```shell
cd sam-app && sam build --cached
```

- `Lambda`のローカル実行

```shell
sam local invoke
```

- ローカルのサーバーを起動

```shell
sam local start-api
```

- `Lambda`のローカル実行

```shell
curl http://127.0.0.1:3000/hello
```

- `AWS SAM CLI Local`を起動する

```shell
sam local start-lambda
```

- `AWS SAM CLI Local`関数を参照するステートマシンを作成する

```shell
aws stepfunctions --endpoint http://localhost:8083 create-state-machine --definition "{\
  \"Comment\": \"A Hello World example of the Amazon States Language using an AWS Lambda Local function\",\
  \"StartAt\": \"HelloWorld\",\
  \"States\": {\
    \"HelloWorld\": {\
      \"Type\": \"Task\",\
      \"Resource\": \"arn:aws:lambda:us-east-1:123456789012:function:HelloWorldFunction\",\
      \"End\": true\
    }\
  }\
}" --name "HelloWorld" --role-arn "arn:aws:iam::012345678901:role/DummyRole"
```

- ローカルステートマシンの実行を開始する

```shell
aws stepfunctions --endpoint-url http://localhost:8083 list-state-machines --query 'stateMachines[*].{name:name,arn:stateMachineArn}' \
  && arn=$(aws stepfunctions --endpoint-url http://localhost:8083 list-state-machines --query 'stateMachines[0].{arn:stateMachineArn}' --output text) \
  && echo ${arn} \
  && runName=$(openssl rand -base64 100 | tr -dc 'a-zA-Z' | fold -w 10 | head -n 1) \
  && executionArn=$(aws stepfunctions --endpoint-url http://localhost:8083 start-execution \
  --state-machine-arn ${arn} \
  --name ${runName} \
  --query 'executionArn' --output text) \
  && aws stepfunctions describe-execution --endpoint-url http://localhost:8083 --execution-arn ${executionArn} --output json
```

## [AWS Step Functions Localでモックテストをする](https://qiita.com/taskforce_Hisui/items/397ed38f0cdae94b0941)

- `ASL`ファイルの取得

```shell
aws stepfunctions describe-state-machine \
  --state-machine-arn arn:aws:states:ap-northeast-1:000000000000:stateMachine:stg-sample-statemachine \
  | jq '.definition | fromjson' > sample-statemachine.asl.json 
```

### ディレクトリ構成

```
.
├── docker-compose.yaml
└── statemachine
    ├── MockConfigFile.json
    ├── aws-stepfunctions-local-credentials.txt
    └── test-sample-statemachine.asl.json
```

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

### `Python`版

```shell
stackName=cdk-step-functions-stack \
 && runName=$(openssl rand -base64 100 | tr -dc 'a-zA-Z' | fold -w 10 | head -n 1) \
 && arn=$(aws cloudformation describe-stacks --stack-name ${stackName} --query 'Stacks[].Outputs[?OutputKey==`cdkstepfunctionsstatemachine2arn`].OutputValue' --output text --profile dev) \
 && executionArn=$(aws stepfunctions start-execution \
   --state-machine-arn ${arn} \
   --name ${runName} \
   --input '{"throw": "1"}' \
   --query 'executionArn' --output text) \
 && echo runName: ${runName} \
 && aws stepfunctions describe-execution --execution-arn ${executionArn} --output text
```

### `C#`

- `CDK`でデプロイ

```shell
cdk deploy --require-approval never
```

```shell
cdk deploy --require-approval never --hotswap
```

- `Step Functions`の実行

```shell
stackName=cdk-step-functions-stack \
 && runName=$(openssl rand -base64 100 | tr -dc 'a-zA-Z' | fold -w 10 | head -n 1) \
 && arn=$(aws cloudformation describe-stacks --stack-name ${stackName} --query 'Stacks[].Outputs[?OutputKey==`cdkstepfunctionsstatemachine3arn`].OutputValue' --output text --profile dev) \
 && executionArn=$(aws stepfunctions start-execution \
   --state-machine-arn ${arn} \
   --name ${runName} \
   --input '{"throw": "1"}' \
   --query 'executionArn' --output text) \
 && echo runName: ${runName} \
 && aws stepfunctions describe-execution --execution-arn ${executionArn} --output text
```

- `Step Functions`の`ASL`の取得

```shell
stackName=cdk-step-functions-stack \
  && arn=$(aws cloudformation describe-stacks --stack-name ${stackName} --query 'Stacks[].Outputs[?OutputKey==`cdkstepfunctionsstatemachine3arn`].OutputValue' --output text --profile dev) \
  && aws stepfunctions describe-state-machine \
  --state-machine-arn ${arn} --query "definition" --output text > statemachine3.asl.tmp.json
```
