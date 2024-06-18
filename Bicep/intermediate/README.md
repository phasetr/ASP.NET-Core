# README

- [中級Bicep](https://learn.microsoft.com/ja-jp/training/paths/intermediate-bicep/)

## はじめに

>Bicep でリソース間の関係について認識されると、提供可能な多くの利点が失われてしまうため、通常は、リソース名を作成しないことをお勧めします。 このオプションは、子リソースを宣言するために他のアプローチのいずれも使用できない場合にのみ使用してください。

最新版`Bicep`のインストール確認。

```shell
az bicep install && az bicep upgrade
```

ログイン。

```shell
az login
```

サンドボックスで作られているサブスクリプションを選んでおく。

```shell
az account set --subscription "Concierge Subscription"
```

コンシェルジェサブスクリプションのIDを取って、
最新のサブスクリプションIDに置き換える。

```shell
az account list \
  --refresh \
  --query "[?contains(name, 'Concierge Subscription')].id" \
  --output table
```

```shell
az account list \
  --refresh \
  --query "[?contains(name, 'Concierge Subscription')].id" \
  --output json | jq '.[]' | az account set --subscription
```

```shell
az configure --defaults  \
  group="learn-21ce50ea-5950-4580-a3d6-066b366d26b7"
```

テンプレートをAzureにデプロイする。

```shell
cd intermediate
az deployment group create --template-file main.bicep
```

1-2分待ったら[Azure portal](https://portal.azure.com/#home)にアクセスしてデプロイを検証する。

Log Analytics ワークスペースを作成する。

```shell
az monitor log-analytics workspace create \
  --workspace-name ToyLogs \
  --location eastus
```

おもちゃのデザインドキュメント用のストレージアカウントを作成する。

```shell
storageaccountname="ptsaname"
az storage account create \
  --name ${storageaccountname} \
  --location eastus
```

追加した`bicep`を実行。

```shell
storageaccountname="ptsaname"
az deployment group create \
  --template-file main.bicep \
  --parameters storageAccountName=${storageaccountname}
```

デプロイチェックのために`Azure portal`へ移動。
何をどう見るかは[このページの記述参照](https://learn.microsoft.com/ja-jp/training/modules/child-extension-bicep-templates/7-exercise-deploy-extension-existing-resources?pivots=cli).
