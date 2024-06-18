# README

- [条件とループを使用して柔軟な Bicep テンプレートを作成する](https://learn.microsoft.com/ja-jp/training/modules/build-flexible-bicep-templates-conditions-loops/)

```shell
az bicep install && az bicep upgrade
az login
```

- `az login --allow-no-subscriptions`

```shell
az account list --refresh --query "[?contains(name, 'Concierge Subscription')].id" --output table
```

```shell
subscriptionId=$(az account list --refresh --query "[?contains(name, 'Concierge Subscription')].id" --output json | jq -r '.[0]')
az account set --subscription ${subscriptionId}
```

```shell
az configure --defaults group=learn-e8ba9567-8b5b-4cd9-99ca-eaaf5ffd7d50
```

```shell
az deployment group create --template-file main.bicep
```
