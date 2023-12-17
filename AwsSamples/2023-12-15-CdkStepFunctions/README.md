# README

- [ステートマシンをローカルでテストする](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/sfn-local.html)
    - `Docker`を使う
    - 参考：[AWS Step Functions Localでモックテストをする](https://qiita.com/taskforce_Hisui/items/397ed38f0cdae94b0941)
- AWS公式の単純なサンプル：[LambdaStep Functions使用するためのステートマシンの作成 AWS CDK](https://docs.aws.amazon.com/ja_jp/step-functions/latest/dg/tutorial-lambda-state-machine-cdk.html)
- [[AWS CDK] Step Functions の Lambda タスクを同期呼び出しする場合の ASL の書き方について紹介・比較する](https://zenn.dev/hassaku63/articles/aefff9ebfee49f)
    - [GitHub](https://github.com/hassaku63/cdk-sfn-example)  
- [AWS Step FunctionsとLambdaを使ってじゃんけんのフローを作ってみた](https://liginc.co.jp/592766)

## `Step Functions Local`

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
  && aws stepfunctions --endpoint-url http://localhost:8083 start-execution --state-machine-arn ${arn}
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
 && aws stepfunctions start-execution \
   --state-machine-arn ${arn} \
   --name ${runName} \
   --input '{"throw": "1"}' \
 && echo runName: ${runName}
```

### `C#`

- `TODO`: `LambdaSample3/src/Function.cs`を適切に修正する必要あり
    - 修正できたら`LambdaSample1`も入力をログ出力して確認しよう

```shell
stackName=cdk-step-functions-stack \
 && runName=$(openssl rand -base64 100 | tr -dc 'a-zA-Z' | fold -w 10 | head -n 1) \
 && arn=$(aws cloudformation describe-stacks --stack-name ${stackName} --query 'Stacks[].Outputs[?OutputKey==`cdkstepfunctionsstatemachine3arn`].OutputValue' --output text --profile dev) \
 && aws stepfunctions start-execution \
   --state-machine-arn ${arn} \
   --name ${runName} \
   --input '{"Input": "1"}' \
 && echo runName: ${runName}
```
