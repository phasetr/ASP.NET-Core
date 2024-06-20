# README

- [Bicep を使用してサブスクリプション、管理グループ、テナントにリソースをデプロイする](https://learn.microsoft.com/ja-jp/training/modules/deploy-resources-scopes-bicep/)

## memo

- targetScope キーワードを使用して Bicep ファイルのスコープを設定する。
- モジュールと scope キーワードを使用して、1 つのデプロイ内でさまざまなスコープにリソースをデプロイする。
- resourceGroup()、subscription()、managementGroup()、tenant() 関数を使用して、特定のスコープをターゲットにする。

## 手順

```shell
az bicep install && az bicep upgrade
```

```shell
az login
```

```shell
az account list --output table
```

テンプレートを`Azure`にデプロイ

```shell
templateFile="main.bicep"
today=$(date +"%d-%m-%Y")
deploymentName="sub-scope-"$today

az deployment sub create \
  --name $deploymentName \
  --location westus \
  --template-file $templateFile
```

```shell
subscriptionId=$(az account show --query 'id' --output tsv)

az policy assignment delete --name 'DenyFandGSeriesVMs' --scope "/subscriptions/$subscriptionId"
az policy definition delete --name 'DenyFandGSeriesVMs' --subscription $subscriptionId
```

```shell
az policy assignment delete --name 'DenyFandGSeriesVMs' --scope "/subscriptions/$subscriptionId"
```

```shell
templateFile="main.bicep"
today=$(date +"%d-%m-%Y")
deploymentName="sub-scope-"$today
virtualNetworkName="rnd-vnet-001"
virtualNetworkAddressPrefix="10.0.0.0/24"

az deployment sub create --name $deploymentName --location westus --template-file $templateFile --parameters virtualNetworkName=$virtualNetworkName virtualNetworkAddressPrefix=$virtualNetworkAddressPrefix
```

```shell
subscriptionId=$(az account show --query 'id' --output tsv)

az policy assignment delete --name 'DenyFandGSeriesVMs' --scope "/subscriptions/$subscriptionId"
az policy definition delete --name 'DenyFandGSeriesVMs' --subscription $subscriptionId
az group delete --name ToyNetworking
```

管理グループの作成

```shell
az account management-group create \
  --name SecretRND \
  --display-name "Secret R&D Projects"
```

## tmpl2

```shell
managementGroupId="SecretRND"
templateFile="main.bicep"
today=$(date +"%d-%m-%Y")
deploymentName="mg-scope-"$today

az deployment mg create --management-group-id $managementGroupId --name $deploymentName --location westus --template-file $templateFile
```

```shell
az account management-group delete --name SecretRND
```
