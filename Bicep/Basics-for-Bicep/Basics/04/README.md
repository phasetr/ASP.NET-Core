# README

- [条件とループを使用して柔軟な Bicep テンプレートを作成する](https://learn.microsoft.com/ja-jp/training/modules/build-flexible-bicep-templates-conditions-loops/)

```shell
az bicep install && az bicep upgrade
az login
```

```shell
az login --allow-no-subscriptions
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
az configure --defaults group="learn-66f6b605-5e9c-40b8-8e10-189bfd23d62e"
```

```shell
az deployment group create \
  --template-file main.bicep \
  --parameters location=eastus
```

admin: phasetr2
pass: testpass0A!

```shell
az deployment group create --template-file main.bicep
```

```shell
az deployment group create --template-file main.bicep --parameters environmentName=Production location=eastus
```

```shell
az deployment group create --template-file main.bicep
```
