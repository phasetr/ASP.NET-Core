# Standalone Blazor WebAssembly with ASP.NET Core Identity

- [Original](https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAssemblyStandaloneWithIdentity)
- Users
   - `leela@contoso.com`/`Passw0rd!`
       - `Administrator`, `Manager`, and `User`
   - `harry@contoso.com`/`Passw0rd!`
       - `User`
- 特に開発環境では`Program.cs`で適切な`URL`が設定されているか確認する
- デプロイは`AWS/CDK/デプロイ・確認`を見ること
- いったんデプロイしたあとの設定
  - `Api`の`appsettings.json`で`ClientUrl`を適切に設定
  - `Blazor`の`appsettings.json`で`ApiBaseAddress`を適切に設定
  - 設定後もう一度デプロイする
  - フロントからの`API`結果取得で実行してうまくいかない場合は`Lambda`のログを確認しよう
  - `Blazor`からうまく実行できない場合は以下の手順に沿って`curl`で`Lambda`を実行してみよう
  - ブラウザからの実行では`CORS`の問題もあるため、それも調べよう- 
- ローカル開発時の注意
  - ルート直下の`compose.yml`で`DynamoDB Local`を`docker compose`で起動する。
    `DynamoDB Local`の管理画面が`http://localhost:8001`で起動する。
  - `Blazor`・`Api`はそれぞれコマンドラインから実行する。
    （`docker compose`で立ち上げるわけではない。）
  - `DynamoDB Local`は次のコマンドで作る

## 参考

- 参考：[CloudFront と API Gateway で SPA の CORS 問題をイイ感じに解決する](https://dev.classmethod.jp/articles/spa-cloudfront-and-api-gateway-voiding-cors/)
  - これは難しそう：頑張って`CORS`対応しよう
  - 参考：CORS対策のフロント・バックエンドの統合
    - <https://dev.to/evnz/single-cloudfront-distribution-for-s3-web-app-and-api-gateway-15c3>
    - GitHub: <https://github.com/evnz/blog-example-single-cf-distribution/tree/main>
- 参考・後で実装：[dynamodbのローカルでのテスト並列実行](https://dev.classmethod.jp/articles/localstack-dynamodb-concurrency/)
- GitHub: [aws-cdk-examples/csharp/apigateway-cognito-lambda-dynamodb at master · aws-samples/aws-cdk-examples](https://github.com/aws-samples/aws-cdk-examples/tree/master/csharp/apigateway-cognito-lambda-dynamodb)
  - `Cognito`まわりはこのリポジトリを参考にした
- GitHub: [aws-samples/aws-cdk-examples: Example projects using the AWS CDK](https://github.com/aws-samples/aws-cdk-examples/tree/master)
- 初め`OAC`でエラーが起きていたため`AWS`に質問を投げた時のメモ
  - `OAC`のエラーは`CloudTrail`のログを見るとわかる。
  - `CloudFormation`含めてログをしっかり見る。
  - 今回は`"errorMessage": "The parameter Name is too big."`だった
  - `OAC`は`Name`は最大64文字まで

## `AWS`

### `Cognito`と`DynamoDB`のデータ初期化

- `Scripts/initCognitoDynamoDB.sh`を実行すること

### コマンドライン上で初期化後の確認

- `read-only`ユーザーグループに割り当てたユーザーでログインする。
- 上記手順で遷移した`URL`から`access_token`を取得する。
  以下のコマンドはコールバック`URL`から取得した`access_token`を貼り付けている。

```shell
stackName=cdk-fullstack-with-auth-stack-dev
aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitohosteduiurldev`].OutputValue' \
  --output text
```

```shell
export AccessToken="<ログイン後URLから取得>"
```

```shell
export AccessToken=""
```

- `ApiGwEndpoint`の`URL`を取得し、`Authorization`ヘッダーに`access_token`を設定して`GET`リクエストを送信する。

```shell
export ApiGwEndpoint=$(aws cloudformation describe-stacks \
  --stack-name cdk-fullstack-with-auth-stack-dev \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthapigwurldev`].OutputValue' \
  --output text)
echo ${ApiGwEndpoint}
```

```shell
curl -H "Authorization: Bearer ${AccessToken}" -i ${ApiGwEndpoint}
```

```shell
curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer ${AccessToken}" -i ${ApiGwEndpoint}
```

- Confirm that you are returned an 200 Success response
- Now invoke the same request with the POST verb and the same `access_token`.
- Confirm that you are returned an HTTP 403 Unauthorized response.
- Log into the Hosted UI as the second user you created following the steps previously described.
- Make a GET request to the API Gateway HTTP Endpoint as you did previously,
  this time using the second user's `access_token` for the bearer token.
- Confirm that you are returned a 200 Success response
- Make a POST request to the API HTTP Endpoint
- Confirm you are returned an HTTP 201 Created response
- Try making a GET or POST request to the endpoint with invalid token
- Observe that you're returned an HTTP 401 Unauthorized response

### `CDK`

- メモ：スタック取得用コマンド

```shell
aws cloudformation describe-stacks --stack-name cdk-fullstack-with-auth-stack-dev
```

#### デプロイ・確認

- `devDeploy.sh`を実行する
- `Cognito`用の初期データ登録は`Cognito`欄を参考にすること

#### 削除

```shell
cdk destroy --profile dev
```

### `SAM`

- `TODO`: 適切な形で`CDK`から`SAM`のテンプレートを書き出す
- テンプレートを書き出したら適切な`SAM`のコマンドを使って各種処理を進める
- [コマンド集の参考ページ](https://zenn.dev/sugikeitter/articles/sam-written-by-cdk)

#### `SAM CLI` with `CDK`

- 利用イメージ：`CDK`は`AWS`リソースを構築するためだけのツールであるため,
  `CDK`がカバーしない・サーバーレスアプリケーションの「ローカルテスト」の機能を`SAM`で補完する
- [Qiita: AWS SAM with CDK](https://qiita.com/nisim/items/b4aaaef4da116f381f51)

> さていよいよ本題です。
> `CDK`で作成したスタックを`SAM CLI`の`local`テストでテストします。
> 具体的には`sam local invoke`コマンドの`-t`オプションの引数に`cdk synth`などで生成されるテンプレート`(cdk.out/hogehogeStack.template.json)`を指定し、
> `cdk`で作成した`yaml`ファイルを`sam`の`cli`でテストします。

```shell
sam local invoke -e events/event.json -t ./cdk.out/cdk-fullstack-with-auth-stack-dev.template.json CdkFunction
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
```

- `SAM`以外のリソースも変更セットを作成せずにそのままデプロイできるため、開発環境で手早くデプロイしたい時に便利。

```shell
sam sync --stack-name [STACK_NAME]
```

## 関連する`AWS`サービスのメモ

### `SAM`

- Zenn: [SAM チュートリアル](https://zenn.dev/shinkano/articles/a4076f3297b444)
- `Cognito`
  - 公式: [Amazon Cognito ID プールを使用して ASP.NET コアアプリケーションから AWS サービスにアクセスする](https://docs.aws.amazon.com/ja_jp/prescriptive-guidance/latest/patterns/access-aws-services-from-an-asp-net-core-app-using-amazon-cognito-identity-pools.html)
  - `Developer's IO`: [AWS SDK for .NET を使って、.NET 6 アプリケーションから Amazon Cognito ユーザープールのユーザー属性を更新する](https://dev.classmethod.jp/articles/aws-sdk-for-net-net-6-amazon-cognito/)
  - サインアップユーザーを`DynamoDB`のトリガーで登録する: [Cognito でサインアップしたユーザーの情報を DynamoDB に保存するには？](https://zenn.dev/tatsurom/articles/cognito-data-to-dynamodb)

#### 実行

```shell
sam local invoke
```

#### デプロイ

- 確認

```shell
sam build && sam validate
```

- 実際にデプロイ

```shell
sam build && sam deploy
```

#### プロジェクトの確認

```shell
sam list --help
```

```shell
sam list stack-outputs
```

#### プロジェクトの削除

```shell
sam delete
```

#### プロジェクトの初期化

- テンプレートは`Serverless API`

```shell
sam init --name SamCognitoLambda --runtime dotnet6
```

- 設定の書き出し

```shell
sam deploy -g
```

### `Cognito+API Gateway+Lambda`で実行権限を動的に制御したい

- [Cognito + API Gateway + Lambda で実行権限を動的に制御したい](https://blog.serverworks.co.jp/2021/12/22/125040#Lambda-関数と-API-Gateway-と-Cognito-Authorizer-を作る)

#### 実行

- `DynamoDB`作成

```shell
aws dynamodb create-table \
    --table-name Users \
    --attribute-definitions \
        AttributeName=tenant,AttributeType=S \
        AttributeName=user_id,AttributeType=S \
    --key-schema \
        AttributeName=tenant,KeyType=HASH \
        AttributeName=user_id,KeyType=RANGE \
    --provisioned-throughput \
        ReadCapacityUnits=5,WriteCapacityUnits=5
```

- テスト用のデータ作成

```shell
aws dynamodb batch-write-item \
    --request-items \
    "{\"Users\": [
        {
            \"PutRequest\": {
                \"Item\": {
                    \"tenant\": {\"S\": \"A\"},
                    \"user_id\": {\"S\": \"A-user01\"},
                    \"email\": {\"S\": \"A-user01@example.com\"}
                }
            }
        },
        {
            \"PutRequest\": {
                \"Item\": {
                    \"tenant\": {\"S\": \"A\"},
                    \"user_id\": {\"S\": \"A-user02\"},
                    \"email\": {\"S\": \"A-user02@example.com\"}
                }
            }
        },
        {
            \"PutRequest\": {
                \"Item\": {
                    \"tenant\": {\"S\": \"B\"},
                    \"user_id\": {\"S\": \"B-user01\"},
                    \"email\": {\"S\": \"B-user01@example.com\"}
                }
            }
        },
        {
            \"PutRequest\": {
                \"Item\": {
                    \"tenant\": {\"S\": \"B\"},
                    \"user_id\": {\"S\": \"B-user02\"},
                    \"email\": {\"S\": \"B-user02@example.com\"}
                }
            }
        },
        {
            \"PutRequest\": {
                \"Item\": {
                    \"tenant\": {\"S\": \"C\"},
                    \"user_id\": {\"S\": \"C-user01\"},
                    \"email\": {\"S\": \"C-user01@example.com\"}
                }
            }
        }
    ]
}"
```

- ユーザープールの作成

```shell
aws cognito-idp create-user-pool \
  --pool-name demo-userpool \
  --schema \
    Name=email,Required=true \
    Name=name,Required=true
```

- 出力のユーザープールのIDをメモする

```shell
USER_POOL_ID=ap-northeast-1_Ug7Z1td0m
```

- テスト用ユーザーの準備

```shell
aws cognito-idp admin-create-user \
    --user-pool-id ${USER_POOL_ID} \
    --username A-user01 \
    --temporary-password P@ssw0rd \
    --message-action SUPPRESS \
    --user-attributes \
        Name=email,Value=A-user01@example.com
```

- パスワード変更

```shell
aws cognito-idp admin-set-user-password \
    --user-pool-id ${USER_POOL_ID} \
    --username A-user01 \
    --password newP@ssw0rd \
    --permanent
```

- ユーザープールとアプリケーションを紐づけるアプリクライアントを作成

```shell
aws cognito-idp create-user-pool-client \
    --user-pool-id ${USER_POOL_ID} \
    --client-name app-web \
    --explicit-auth-flows \
        ALLOW_ADMIN_USER_PASSWORD_AUTH \
        ALLOW_REFRESH_TOKEN_AUTH
```

- クライアントIDを記録

```shell
CLIENT_ID=382t2bjrcuv5prssvjalv70n4k
```

- グループ作成

```shell
aws cognito-idp create-group \
    --group-name A \
    --user-pool-id ${USER_POOL_ID}
```

- `A-user01`を`A`に所属させる

```shell
aws cognito-idp admin-add-user-to-group \
    --user-pool-id ${USER_POOL_ID} \
    --username A-user01 \
    --group-name A
```

### How to Implement ASP.NET Core Identity using DynamoDB

- Blog post: [How to Implement ASP.NET Core Identity using DynamoDB](https://dynamodbdotnet.wordpress.com/2021/03/14/how-to-implement-asp-net-core-identity-using-dynamodb/)
- GitHub: [DynamoDBIdentity](https://github.com/paulverger/DynamoDBIdentity/tree/master)

#### 起動

- `docker compose up`で`DynamoDB Local`と管理GUIを立ち上げる。
- テーブル生成

```shell
aws dynamodb create-table \
  --table-name ApplicationUser \
  --attribute-definitions \
    AttributeName=NormalizedUserName,AttributeType=S \
  --key-schema \
    AttributeName=NormalizedUserName,KeyType=HASH \
  --provisioned-throughput \
    ReadCapacityUnits=5,WriteCapacityUnits=5 \
  --endpoint-url=http://localhost:8000
aws dynamodb create-table \
  --table-name ApplicationRole \
  --attribute-definitions \
    AttributeName=RoleId,AttributeType=N \
  --key-schema \
    AttributeName=RoleId,KeyType=HASH \
  --provisioned-throughput \
    ReadCapacityUnits=5,WriteCapacityUnits=5 \
  --endpoint-url=http://localhost:8000
aws dynamodb create-table \
  --table-name UserToRoles \
  --attribute-definitions \
    AttributeName=NormalizedUserName,AttributeType=S \
    AttributeName=RoleId,AttributeType=N \
  --key-schema \
    AttributeName=NormalizedUserName,KeyType=HASH \
    AttributeName=RoleId,KeyType=RANGE \
  --provisioned-throughput \
    ReadCapacityUnits=5,WriteCapacityUnits=5 \
  --endpoint-url=http://localhost:8000
```

- データ投入

```shell
aws dynamodb put-item \
  --table-name ApplicationRole \
  --item '{
    "RoleId": {
      "N": "1"
    },
    "NormalizedRoleName": {
      "S": "EMPLOYEE"
    },
    "RoleName": {
      "S": "Employee"
    }
  }' \
  --return-consumed-capacity TOTAL \
  --endpoint-url=http://localhost:8000
```

#### テーブル構造

- `ApplicationUser`
  - `Partition Key`: `NormalizedUserName`
- `ApplicationRole`
  - `Partition Key`: `RoleId`
- `UserToRoles`
  - `Partition Key`: `NormalizedUserName`
  - `Sort Key`: `RoleId`

## Original

This sample app demonstrates how to use the built-in ASP.NET Core Identity capabilities from a standalone Blazor WebAssembly app.

For more information, see [Secure ASP.NET Core Blazor WebAssembly with ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity).

### Steps to run the sample

1. Clone this repository or download a ZIP archive of the repository. For more information, see [How to download a sample](https://learn.microsoft.com/aspnet/core/introduction-to-aspnet-core#how-to-download-a-sample).

1. The default and fallback URLs for the two apps are:

   * `Backend` app (`BackendUrl`): `https://localhost:7211` (fallback: `https://localhost:5001`)
   * `BlazorWasmAuth` app (`FrontendUrl`): `https://localhost:7171` (fallback: `https://localhost:5002`)

   You can use the existing URLs or update them in the `appsettings.json` file of each project with new `BackendUrl` and `FrontendUrl` endpoints:

   * `appsettings.json` file in the root of the `Backend` app.
   * `wwwroot/appsettings.json` file in the `BlazorWasmAuth` app.

1. If you plan to run the apps using the .NET CLI with `dotnet run`, note that first launch profile in the launch settings file is used to run an app, which is the insecure `http` profile (HTTP protocol). To run the apps securely (HTTPS protocol), take ***either*** of the following approaches:

   * Pass the launch profile option to the command when running the apps: `dotnet run -lp https`.
   * In the launch settings files (`Properties/launchSettings.json`) ***of both projects***, rotate the `https` profiles to the top, placing them above the `http` profiles.

   If you use Visual Studio to run the apps, Visual Studio automatically uses the `https` launch profile. No action is required to run the apps securely when using Visual Studio.

1. Run the `Backend` and `BlazorWasmAuth` apps.

1. Navigate to the `BlazorWasmAuth` app at the `FrontendUrl`.

1. Register a new user using the **Register** link in the upper-right corner of the app's UI or use one of the preregistered test users:

   * `leela@contoso.com` (Password: `Passw0rd!`). Leela has `Administrator`, `Manager`, and `User` roles and can access the private manager page but not the private editor page of the app. She can process data with both forms on the data processing page.
   * `harry@contoso.com` (Password: `Passw0rd!`). Harry only has the `User` role and can't access the manager and editor pages. He can only process data with the first form on the data processing page.

1. Log in with the user.

1. Navigate to the private page (`Components/Pages/PrivatePage.razor` at `/private-page`) that only authenticated users can reach. A link to the page appears in the navigation sidebar after the user is authenticated. Navigate to the private manager and editor pages to explore how the user's roles influence the pages that they can visit. Navigate to the data processing page (`Components/Pages/DataProcessing.razor` at `/data-processing`) to experience authenticated and authorized data processing web API calls.

1. Log out of the app.
