# README

- [Bicep を使用するために Azure リソースと JSON ARM テンプレートを移行する](https://learn.microsoft.com/ja-jp/training/modules/migrate-azure-resources-bicep/)

```shell
az deployment group what-if \
  --mode Complete \
  --resource-group ToyTruck \
  --template-file main.bicep \
  --parameters main.parameters.production.json
```

リソースのクリーンアップ

```shell
az group delete --resource-group ToyTruck --yes --no-wait
```
