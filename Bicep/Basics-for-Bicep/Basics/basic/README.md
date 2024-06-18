# README

- [Bicepの基礎](https://learn.microsoft.com/ja-jp/training/paths/fundamentals-bicep/)
- [Bicepを使用したコードとしてのインフラストラクチャの概要](https://learn.microsoft.com/ja-jp/training/modules/introduction-to-infrastructure-as-code-using-bicep/)

```shell
az deployment group create --template-file main.bicep --resource-group storage-resource-group
```

```shell
az bicep build --file main.bicep
```

```shell
az bicep decompile --file main.json
```

- [演習-Bicep テンプレートでリソースを定義する](https://learn.microsoft.com/ja-jp/training/modules/build-first-bicep-template/4-exercise-define-resources-bicep-template?pivots=cli)
- `Azure`へのサインイン

```shell
az bicep install && az bicep upgrade
az login
az account set --subscription "Concierge Subscription"
az account list --refresh --query "[?contains(name, 'Concierge Subscription')].id" --output table
```

- サブスクリプションIDを利用

```shell
subscriptionId=18974119-7a45-4077-9932-f95c83cee0e3
```

```shell
az account set --subscription ${subscriptionId}
```

```shell
az configure --defaults group=learn-2498a680-3e77-4ee6-84d3-c2466a1138c8
```

```shell
az deployment group create --template-file main.bicep
```

- コマンドラインからデプロイ確認

```shell
az deployment group list --output table
```

```shell
az deployment group create --template-file main.bicep --parameters environmentType=nonprod
```

- 「パラメーターを使用して再利用可能な Bicep テンプレートを構築する」での記述

```shell
subscriptionId=9a0792f6-a232-44c6-bc38-e18f53792073
```

```shell
az account set --subscription ${subscriptionId}
```

```shell
az configure --defaults group=learn-f8fd88e3-9b9f-4820-bb2f-a72ea1b3910c
```

```shell
az deployment group create --template-file main.bicep
```

- `Chapter6`

```shell
az deployment group create \
  --template-file main.bicep \
  --parameters main.parameters.dev.json
```

## 初めてのBicepテンプレートを作成する

- <https://learn.microsoft.com/ja-jp/training/modules/build-first-bicep-template/>
