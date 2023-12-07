# README

- [クイック スタート: Bicep を使用して Azure Functions リソースを作成してデプロイする](https://learn.microsoft.com/ja-jp/azure/azure-functions/functions-create-first-function-bicep?tabs=CLI)

```shell
az group create --name exampleRG --location eastus
```

```shell
location=$(az group show --name exampleRG | jq -r '.["location"]')
echo ${location}
```

```shell
az deployment group create --resource-group exampleRG --template-file main.bicep --parameters appInsightsLocation=${location}
```

```shell
az resource list --resource-group exampleRG
```

- 関数アプリのウェルカム ページにアクセスするためにURLを調べる

```shell
host=$(az resource list --resource-group exampleRG \
  --output json \
  | jq -r '[.[] | select(.kind == "functionapp")][0].name')
echo ${host}
echo https://${host}.azurewebsites.net
```

- 上記の`URL`にアクセスして環境ができているか確認する
- 次のコマンドでリソースグループを削除する

```shell
az group delete --name exampleRG
```
