# Standalone Blazor WebAssembly with ASP.NET Core Identity

- [Original](https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAssemblyStandaloneWithIdentity)
- ユーザーは`api`の`register`を実行して登録する。フロントの`Blazor`からも登録できる。
- `CDK`でのデプロイは`AWS/CDK/デプロイ・確認`を見ること
- `devDeploy.sh`でデプロイできる
  - このスクリプト実行でフロント・`API`プロジェクトの`appsettings.json`・`appsettings.Development.json`を設定するため、
    うまく動かない場合はまずはスクリプトを実行すること。
  - 特に`appsettings.json`は実際の`AWS`での値を前提にした、スクリプトによる設定を前提に`.gitignore`に入れているため、
    プロジェクトにコミットされていない点に注意する。
- ローカル開発時の注意
  - ルート直下の`compose.yml`で`DynamoDB Local`を`docker compose`で起動する。
    `DynamoDB Local`の管理画面が`http://localhost:8001`で起動する。
  - `Blazor`・`Api`はそれぞれコマンドラインから`dotnet watch`で実行する。
    （`docker compose`で立ち上げるわけではない。）

## 開発環境での起動

- `開発環境構築`の内容を実行して初期化すること。

```shell
docker compose up -d
```

- `Blazor`・`Api`をそれぞれ別のターミナルで起動する。

```shell
dotnet watch run --project BlazorWasmAuth
dotnet watch run --project Api
````

## 開発環境構築

```shell
docker compose up -d
dotnet run --project InitDynamoDb
```

## `AWS`

### `DynamoDB`のデータ初期化（ローカルだけ）

- 本番環境用の設定（特に`GSI`）は`CDK`で対応する：認証周りがうまく動かないなら利用ライブラリの[AspNetCore.Identity.AmazonDynamoDB](https://github.com/ganhammar/AspNetCore.Identity.AmazonDynamoDB/blob/main/src/AspNetCore.Identity.AmazonDynamoDB/Setup/DynamoDbTableSetup.cs#L52)と比較して設定を確認すること。

```shell
dotnet run --project InitDynamoDb
```

- 上記コマンドで正しく`GSI`が設定されたか確認する。

```shell
aws dynamodb describe-table --table-name ba-ddb-local --endpoint-url http://localhost:8000 --output text
```

- 本番環境の確認は次の通り

```shell
aws dynamodb describe-table --table-name ba-ddb-dev --output text
```

### `CDK`

- メモ：スタックの情報取得用コマンド

```shell
aws cloudformation describe-stacks --stack-name ba-dev --output text
```

#### デプロイ・確認

- `devDeploy.sh`を実行する

#### 削除

```shell
cdk destroy --profile dev
```
