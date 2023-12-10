# README

- [【備忘録】初めてのAzureFunctionsのデプロイ](https://qiita.com/gsy0911/items/61198607476ac686ce6f)
- [クイックスタート: `Azure`でコマンドラインから`C#`関数を作成する](https://learn.microsoft.com/ja-jp/azure/azure-functions/create-first-function-cli-csharp?tabs=macos%2Cazure-cli)
- 参考:[サーバーレスコード実行用の関数アプリを作成する](https://learn.microsoft.com/ja-jp/azure/azure-functions/scripts/functions-cli-create-serverless)

## `Azure`の操作と`Azure Functions`のデプロイ

- `Azure CLI`のインストール

```shell
brew update && brew install azure-cli
```

- `Azure Function Core Tools`のインストール

```shell
brew tap azure/functions
brew install azure-functions-core-tools@4
brew link --overwrite azure-functions-core-tools@4
```

- `Azure`に`AzureCLI`を利用してログイン

```shell
az login
```

- `Azure CLI`と`Azure Functions Core Tools`のバージョン確認

```shell
az --version
func --version
```

- ローカル関数プロジェクトを作成する

```shell
func init LocalFunctionProj --worker-runtime dotnet-isolated --target-framework net8.0
```

```shell
cd LocalFunctionProj
```

```shell
func new --name HttpExample --template "HTTP trigger" --authlevel "anonymous"
```

- `Azure Functions`用のリソースグループを作る
  - リソースグループ・ストレージアカウント・関数アプリを作る

```shell
resourceGroupName="AzureFunctionsQuickstart-rg"
location="japaneast"
storageName="ptstoragex"
functionAppName="ptfunctionappx"
```

- 変数設定の確認

```shell
echo ${resourceGroupName}
echo ${location}
echo ${storageName}
echo ${functionAppName}
```

- リソースグループを作る

```shell
az group create --name ${resourceGroupName} --location ${location}
```

- ストレージアカウントを作成する

```shell
az storage account create --name ${storageName} \
  --location ${location} \
  --resource-group ${resourceGroupName} \
  --sku Standard_LRS \
  --allow-blob-public-access false
```

- 関数アプリの作成

```shell
az functionapp create \
  --resource-group ${resourceGroupName} \
  --consumption-plan-location ${location} \
  --runtime dotnet-isolated \
  --functions-version 4 \
  --name ${functionAppName} \
  --storage-account ${storageName}
```

- 関数プロジェクトをデプロイ

```shell
func azure functionapp publish ${functionAppName}
```

- 関数アプリを呼び出したあとストリーミングログを表示する

```shell
func azure functionapp logstream ${functionAppName}
```

- リソースをクリーンアップする

```shell
az group delete --name ${resourceGroupName}
```

## 関数のローカル実行

- `LocalFunctionProj`に移動 
- `func start`コマンドを実行

```shell
func start
```
