# README

- [モジュールを使用してコンポーザブルな Bicep ファイルを作成する](https://learn.microsoft.com/ja-jp/training/modules/create-composable-bicep-files-using-modules/)

```shell
az bicep install && az bicep upgrade
az login
az account list --refresh --query "[?contains(name, 'Concierge Subscription')].id" --output table
```

```shell
subscriptionId=$(az account list --refresh --query "[?contains(name, 'Concierge Subscription')].id" --output json | jq -r '.[0]')
echo ${subscriptionId}
```

```shell
az account set --subscription ${subscriptionId}
```

```shell
az configure --defaults group=learn-e8ba9567-8b5b-4cd9-99ca-eaaf5ffd7d50
az deployment group create --template-file main.bicep
```
