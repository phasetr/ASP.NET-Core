# README

- [再利用可能なインフラストラクチャ コードのライブラリをテンプレート スペックを使用して発行する](https://learn.microsoft.com/ja-jp/training/modules/arm-template-specs/)

## ユニット2/9 テンプレートスペックについて

## ユニット3/9 テンプレート スペックを作成、発行する

テンプレート スペックを作成するには、`az ts create`コマンド

```shell
az ts create \
  --name StorageWithoutSAS \
  --location westus \
  --display-name "Storage account with SAS disabled" \
  --description "This template spec creates a storage account, which is preconfigured to disable SAS authentication." \
  --version 1.0 \
  --template-file main.bicep
```

## ユニット 4/9 テンプレート スペックをデプロイする

```shell
az deployment group create \
  --template-spec "/subscriptions/f0750bbe-ea75-4ae5-b24d-a92ca601da2c/resourceGroups/SharedTemplates/providers/Microsoft.Resources/templateSpecs/StorageWithoutSAS"
```

## ユニット 5/9 演習 - テンプレート スペックを作成してデプロイする

```shell
az bicep install && az bicep upgrade
```

```shell
az login
```

```shell
az account set --subscription "Concierge Subscription"
```

```shell
az account list \
  --refresh \
  --query "[?contains(name, 'Concierge Subscription')].id" \
  --output json | jq '.[]' | az account set --subscription
```

```shell
az configure --defaults group="learn-a1a6d821-9a11-41bc-a582-76057d004fcd"
```

```shell
az ts create \
  --name ToyCosmosDBAccount \
  --location westus \
  --display-name "Cosmos DB account" \
  --description "This template spec creates a Cosmos DB account that meets our company's requirements." \
  --version 1.0 \
  --template-file main.bicep
```

```shell
id=$(az ts show \
  --name ToyCosmosDBAccount \
  --resource-group "learn-a1a6d821-9a11-41bc-a582-76057d004fcd" \
  --version "1.0" \
  --query "id")
az deployment group create --template-spec $id
```

## ユニット6/9 テンプレート スペックを管理する

テンプレート スペックのバージョンを一覧表示

```shell
az ts show \
  --resource-group MyResourceGroup \
  --name MyTemplateSpec
```

## ユニット7/9 演習 - テンプレート スペックを更新してバージョンを管理する

```shell
az ts create \
  --name ToyCosmosDBAccount \
  --version 2.0 \
  --version-description "Adds Cosmos DB role-based access control." \
  --template-file main.bicep
```

```shell
id=$(az ts show \
  --name ToyCosmosDBAccount \
  --resource-group "learn-a1a6d821-9a11-41bc-a582-76057d004fcd" \
  --version "2.0" \
  --query "id")
az deployment group create \
  --template-spec $id \
  --parameters roleAssignmentPrincipalId="d68d19b3-d7ef-4ae9-9ee4-90695a4e417d"  
```
