# Standalone Blazor WebAssembly with ASP.NET Core Identity

- [Original](https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAssemblyStandaloneWithIdentity)
- ユーザーは`api`の`register`を実行して登録する。フロントの`Blazor`からも登録できる。
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

## `AWS`

### `DynamoDB`のデータ初期化

```shell
dotnet run --project InitDynamoDb
```

### コマンドライン上で初期化後の確認

- `read-only`ユーザーグループに割り当てたユーザーでログインする。
- 上記手順で遷移した`URL`から`access_token`を取得する。
  以下のコマンドはコールバック`URL`から取得した`access_token`を貼り付けている。

```shell
stackName=ba-dev
aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --output text
```

### `CDK`

- メモ：スタック取得用コマンド

```shell
aws cloudformation describe-stacks --stack-name ba-dev --output text
```

#### デプロイ・確認

- `devDeploy.sh`を実行する

#### 削除

```shell
cdk destroy --profile dev
```
