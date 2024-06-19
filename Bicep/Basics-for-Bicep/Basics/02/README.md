# README

- [初めての Bicep テンプレートを作成する](https://learn.microsoft.com/ja-jp/training/modules/build-first-bicep-template/)

## 概要

- `Azure App Service`を構築する
- `Bicep`コードから`Azure`リソースをデプロイするためのリソース定義。
- テンプレートを再利用できるようにするためのパラメーター。
- テンプレートを簡単に記述してデプロイできるようにするための変数と式。
- テンプレートを複数のファイルに構造化するのに役立つモジュール。
- お客様のインフラストラクチャをデプロイしているユーザーまたは何らかに、テンプレートやモジュールからデータを送信するために出力します。

## メモ

>シンボル名は Bicep テンプレート内でのみ使用され、Azure には表示されません。 リソース名 は Azure に表示されます。

`Bicep`のインストール確認

```shell
az bicep install && az bicep upgrade
```

Azureへのログイン

```shell
az login --allow-no-subscriptions
```

規定のサブスクリプションを設定

```shell
az account set --subscription "Concierge Subscription"
```

```shell
az account list \
  --refresh \
  --query "[?contains(name, 'Concierge Subscription')].id" \
  --output table
```

```shell
az deployment group create \
  --template-file main.bicep \
  --parameters environmentType=nonprod
```
