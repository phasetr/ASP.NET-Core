# README

- [プライベート レジストリを使用して Bicep モジュールを共有します](https://learn.microsoft.com/ja-jp/training/modules/share-bicep-modules-using-private-registries/)

## ユニット1/9　はじめに

## ユニット2/9 モジュール レジストリについて理解する

## ユニット3/9 演習 - レジストリを作成する

```shell
az bicep install && az bicep upgrade
```

```shell
az login --allow-no-subscriptions
```

```shell
az account set --subscription "Concierge Subscription"
```

```shell
sid=$(az account list \
   --refresh \
   --query "[?contains(name, 'Concierge Subscription')].id" \
   --output json | jq '.[]')
az account set --subscription ${sid}
```

```shell
az configure --defaults group="learn-15965171-5069-4251-8a09-a11045b35301"
```

```shell
crn="phasetrtest"
az acr create --name ${crn} --sku Basic --location westus
```

```shell
crn="phasetrtest"
az acr repository list --name ${crn}
```

## ユニット 4/9 プライベート レジストリにモジュールを発行する

## ユニット 5/9 演習 - モジュールをレジストリに発行する

```shell
crn="phasetrtest"
az bicep publish --file website.bicep --target "br:${crn}.azurecr.io/website:v1"
az bicep publish --file cdn.bicep --target "br:${crn}.azurecr.io/cdn:v1"
```

```shell
crn="phasetrtest"
az acr repository list --name ${crn}
```

## ユニット6/9 プライベート レジストリからモジュールを使用する

## ユニット7/9 演習 - レジストリからのモジュールを使用する

```shell
az bicep build --file main.bicep
```

```shell
az deployment group create \
  --template-file main.bicep
```
