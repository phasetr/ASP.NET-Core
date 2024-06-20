# README

- [モジュールを使用してコンポーザブルな Bicep ファイルを作成する](https://learn.microsoft.com/ja-jp/training/modules/create-composable-bicep-files-using-modules/)

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
  --output json \
  | jq '.[]' | az account set --subscription
```

```shell
az configure --defaults group="learn-fb08df15-ae14-4e0b-a221-587ad9a01a94"
```

```shell
az deployment group create --template-file main.bicep
```
