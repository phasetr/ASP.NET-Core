# README

## memo

- [チュートリアル: Bicep を介して、Azure Cosmos DB for NoSQL、マネージド ID、AKS を使用する ASP.NET Web アプリケーションをデプロイする](https://learn.microsoft.com/ja-jp/azure/cosmos-db/nosql/tutorial-deploy-app-bicep-aks)

## `azd`

- <https://learn.microsoft.com/ja-jp/azure/developer/azure-developer-cli/overview>

## クイックスタート: Bicep を使用して Azure Cosmos DB とコンテナーを作成する

- <https://learn.microsoft.com/ja-jp/azure/cosmos-db/nosql/quickstart-template-bicep?tabs=CLI>

ここにある`bicep`ファイルでは次の3種類の`Azure`リソースが定義されている。

- `Microsoft.DocumentDB/databaseAccounts`: Azure Cosmos DBアカウント
- `Microsoft.DocumentDB/databaseAccounts/sqlDatabases`: Azure Cosmos DBデータベース
- `Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers`: Azure Cosmos DBコンテナー

```shell
echo ${resourceGroupName}
```

```shell
az group create --name ${resourceGroupName} --location eastus
az deployment group create \
  --resource-group ${resourceGroupName} \
  --template-file infra/main.bicep \
  --parameters primaryRegion=eastus secondaryRegion=westus
```

デプロイの検証

```shell
az resource list --resource-group ${resourceGroupName}
```

削除は記述の最後を参照すること.

## チュートリアル: Azure Cosmos DB for NoSQL で .NET コンソール アプリケーションを開発する

**重要な注意**：2024-08-07時点でこのチュートリアルには間違いがある。
具体的には「データベース`cosmicworks`を作ったあと、
パーティションキーに`/address/country`と`/address/state`を指定したコンテナ`customers`を作る必要がある。
以下の手順でも追記している。

- [上記ページ](https://learn.microsoft.com/ja-jp/azure/cosmos-db/nosql/tutorial-dotnet-console-app)

Cosmos DBのアカウント名を取得.

```shell
cdbAcc=$(az cosmosdb list --resource-group ${resourceGroupName} --query "[].name" --output tsv)
echo ${cdbAcc}
```

URI取得

```shell
uri=$(az cosmosdb show --name ${cdbAcc} --resource-group ${resourceGroupName} --query documentEndpoint --output tsv)
echo ${uri}
```

Cosmos DBのアカウント情報取得

```shell
pKey=$(az cosmosdb keys list --name ${cdbAcc} --resource-group ${resourceGroupName} --type keys --query "primaryMasterKey" --output tsv)
echo ${pKey}
```

チュートリアルに沿って`Azure Portal`からデータベースを作る。
データベース`cosmicworks`を作ったあと、
パーティションキーに`/address/country`と`/address/state`を指定したコンテナ`customers`を作ること。
2024-08-07時点でのチュートリアルにはコンテナ作成手順が抜けている。

TODO：せめて`azure cli`で作りたい. 本当は`Bicep`で直接作りたい.

```shell
dbId=cosmicworks
```

Cosmos DB アカウントの設定を確認

```shell
az cosmosdb show --name ${cdbAcc} --resource-group ${resourceGroupName}
```

権限の確認

```shell
az cosmosdb sql role definition list --account-name ${cdbAcc} --resource-group ${resourceGroupName}
```

Cosmos DBアカウントのリソースIDを取得

```shell
cdbResourceId=$(az cosmosdb show --name ${cdbAcc} --resource-group ${resourceGroupName} --query "id" --output tsv)
echo ${cdbResourceId}
```

ロールの割り当てを表示

```shell
az role assignment list --scope ${cdbResourseId}
```

サブスクリプションIDの確認

```shell
subscriptionId=$(az account show --query id --output tsv)
echo ${subscriptionId}
```

ロールの割り当て確認

```shell
az role assignment list --scope /subscriptions/${subscriptionId}/resourceGroups/${resourceGroupName}/providers/Microsoft.DocumentDB/databaseAccounts/${cdbAcc}
az role assignment list /subscriptions/${subscriptionId}/resourceGroups/${resourceGroupName}/providers/Microsoft.DocumentDB/databaseAccounts/${cdbAcc}
```

## `.NET`コンソールアプリケーション開発

```shell
mkdir App
cd App
dotnet new console --langVersion preview
dotnet add package Microsoft.Azure.Cosmos --version 3.31.1-preview
dotnet add package System.CommandLine --prerelease
dotnet add package Humanizer
dotnet build
```

- 再度動かしたい場合は`CosmosHandler.cs`のコンストラクターで`CosmosClient`の初期化のエンドポイントとキーに適切な値を入れること。入れるべき値は`Azure Portal`の[キー]ページの[URI]フィールドと[PRIMARY KEY]フィールド。

```shell
dotnet run --project App -- --name 'Mica Pereira' --state 'Washington' --country 'United States'
```

## リソースの削除

```shell
az group delete --name ${resourceGroupName}
```
