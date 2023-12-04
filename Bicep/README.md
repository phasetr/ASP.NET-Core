# README

- Azure Portal: <https://portal.azure.com/>
- [Bicepのドキュメント](https://learn.microsoft.com/ja-jp/azure/azure-resource-manager/bicep/)
- [Bicep関数](https://learn.microsoft.com/ja-jp/azure/azure-resource-manager/bicep/bicep-functions)
- [Bicepの基礎](https://learn.microsoft.com/ja-jp/training/paths/fundamentals-bicep/)
- 公式：[Bicep CLIコマンド](https://learn.microsoft.com/ja-jp/azure/azure-resource-manager/bicep/bicep-cli)
- Book: [Infrastructure as Code with Azure Bicep](https://www.packtpub.com/product/infrastructure-as-code-with-azure-bicep/9781801813747)
  - GitHub: [PacktPublishing / Infrastructure-as-Code-with-Azure-Bicep](https://github.com/PacktPublishing/Infrastructure-as-Code-with-Azure-Bicep)

## Bicepの基礎

- [Bicepを使用したコードとしてのインフラストラクチャの概要](https://learn.microsoft.com/ja-jp/training/modules/introduction-to-infrastructure-as-code-using-bicep/)

```shell
az deployment group create --template-file basic/main.bicep --resource-group storage-resource-group
```

```shell
az bicep build --file basic/main.bicep
```

```shell
az bicep decompile --file basic/main.json
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
subscriptionId=2645d18f-f656-46b4-b46b-69edcb85b29d
```

```shell
az account set --subscription ${subscriptionId}
```

```shell
az configure --defaults group=learn-2498a680-3e77-4ee6-84d3-c2466a1138c8
```

```shell
az deployment group create --template-file basic/main.bicep
```

- コマンドラインからデプロイ確認

```shell
az deployment group list --output table
```

```shell
az deployment group create --template-file basic/main.bicep --parameters environmentType=nonprod
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
az deployment group create --template-file basic/main.bicep
```

### 初めてのBicepテンプレートを作成する

- <https://learn.microsoft.com/ja-jp/training/modules/build-first-bicep-template/>

## 子リソースの定義

- [子リソースの定義](https://learn.microsoft.com/ja-jp/training/modules/child-extension-bicep-templates/4-exercise-define-child-resources?pivots=cli)

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
az account list --refresh --query "[?contains(name, 'Concierge Subscription')].id" --output table
```

```shell
az account set --subscription {your subscription ID}
```

```shell
az configure --defaults group=learn-8c155418-54cc-416e-b80d-9165018a82cc
```

```shell
az deployment group create --template-file child-resource/main.bicep
```

```shell
az monitor log-analytics workspace create --workspace-name ToyLogs --location eastus
```

```shell
storageaccountname=20231203storageaccount
```

```shell
az storage account create --name ${storageaccountname} --location eastus
```

```shell
az deployment group create --template-file child-resource/main.bicep --parameters storageAccountName=${storageaccountname}
```

```shell
az deployment group delete --template-file child-resource/main.bicep --parameters storageAccountName=${storageaccountname}
```

```shell
az deployment group what-if --template-file child-resource/main.bicep --parameters storageAccountName=${storageaccountname}
```
