# README

- [パラメーターを使用して再利用可能な Bicep テンプレートを構築する](https://learn.microsoft.com/ja-jp/training/modules/build-reusable-bicep-templates-parameters/)

## memo

> Azure Key Vault からデータベースの資格情報を安全に取得しながら、環境ごとに個別のパラメーター値のセットを作成します。
>
> デプロイによって変わる設定には、パラメーターを使ってみてください。 変数やハードコーディングされた値は、デプロイによって変わらない設定に使用できます。
>
> テンプレート内のパラメーターには既定値を割り当てることができます。
>
>オブジェクト パラメーターにはリソース タグの使用が適しています。

## 手順

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
az account list \
  --refresh \
  --query "[?contains(name, 'Concierge Subscription')].id" \
  --output json \
  | jq '.[]' | az account set --subscription
```

```shell
az configure --defaults group="learn-8415507b-a61d-4983-8fd8-7c84de6f6010"
```

```shell
az deployment group create --template-file main.bicep
```

```shell
az deployment group create \
  --template-file main.bicep \
  --parameters main.parameters.dev.json
```


```shell
az deployment group create --template-file main.bicep --parameters main.parameters.json
```

login: phasetr
password: mytest0A!

```shell
keyVaultName='YOUR-KEY-VAULT-NAME'
read -s -p "Enter the login name: " login
read -s -p "Enter the password: " password

az keyvault create --name $keyVaultName --location eastus --enabled-for-template-deployment true
az keyvault secret set --vault-name $keyVaultName --name "sqlServerAdministratorLogin" --value $login --output none
az keyvault secret set --vault-name $keyVaultName --name "sqlServerAdministratorPassword" --value $password --output none
```

```shell
az keyvault show --name $keyVaultName --query id --output tsv
```
