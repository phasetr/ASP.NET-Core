# README

- [クイック スタート: Bicep を使用して Azure Functions リソースを作成してデプロイする](https://learn.microsoft.com/ja-jp/azure/azure-functions/functions-create-first-function-bicep?tabs=CLI)

```shell
az group create --name exampleRG --location eastus
```

```shell
az deployment group create --resource-group exampleRG --template-file main.bicep --parameters appInsightsLocation=eastus
```

```shell
az resource list --resource-group exampleRG
```

- 削除

```shell
az group delete --name exampleRG
```
