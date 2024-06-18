# README

- Bicep の基礎 / Bicep を使用したコードとしてのインフラストラクチャの概要
- [Infrastructure as Code とは](https://learn.microsoft.com/ja-jp/training/modules/introduction-to-infrastructure-as-code-using-bicep/2-what-infrastructure-code)

```shell
az group create --name storage-resource-group --location eastus
```

上記コマンドでできた対象の削除

```shell
az group delete --name storage-resource-group --location eastus
```

## 命令型コード

```shell
#!/usr/bin/env bash
az group create \
  --name storage-resource-group \
  --location eastus

az storage account create \
  --name mystorageaccount \
  --resource-group storage-resource-group \
  --location eastus \
  --sku Standard_LRS \
  --kind StorageV2 \
  --access-tier Hot \
  --https-only true
```

[このページから](https://learn.microsoft.com/ja-jp/training/modules/introduction-to-infrastructure-as-code-using-bicep/5-how-bicep-works)

```shell
az deployment group create --template-file main.bicep --resource-group storage-resource-group
```
