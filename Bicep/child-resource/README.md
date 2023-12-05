# README

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
az deployment group create --template-file main.bicep
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
az deployment group create --template-file main.bicep --parameters storageAccountName=${storageaccountname}
```

```shell
az deployment group delete --template-file main.bicep --parameters storageAccountName=${storageaccountname}
```

```shell
az deployment group what-if --template-file main.bicep --parameters storageAccountName=${storageaccountname}
```
