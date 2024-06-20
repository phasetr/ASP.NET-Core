# README

- [デプロイ スクリプトを使用して Bicep および ARM テンプレートを拡張する](https://learn.microsoft.com/ja-jp/training/modules/extend-resource-manager-template-deployment-scripts/)

## ユニット3/7 演習 - ARM テンプレートにデプロイ スクリプトを追加する

```shell
az bicep install && az bicep upgrade
```

```shell
az login
```

```shell
az account list --output table
```

```shell
az account set --subscription "Azure subscription 1"
```

```shell
resourceGroupName="learndeploymentscript_exercise_1"
az group create --location eastus --name $resourceGroupName
```

```shell
templateFile="main.bicep"
today=$(date +"%d-%m-%Y")
deploymentName="deploymentscript-"$today

az deployment group create --resource-group $resourceGroupName --name $deploymentName --template-file $templateFile
```

テンプレートの結果の確認

```shell
uri=$(az deployment group show --resource-group $resourceGroupName --name $deploymentName --query 'properties.outputs.fileUri.value' --output tsv)
curl $uri
```

```shell
az deployment-scripts show-log --resource-group $resourceGroupName --name CopyConfigScript
```

リソースグループをクリーンアップ

```shell
az group delete --name $resourceGroupName
```

## ユニット5/7 演習 - デプロイ スクリプトにパラメーターを追加する

`tmpl2`の`main.bicep`を利用

```shell
resourceGroupName="learndeploymentscript_exercise_2"
az group create --location eastus --name $resourceGroupName
```

```shell
templateFile="main.bicep"
templateParameterFile="azuredeploy.parameters.json"
today=$(date +"%d-%m-%Y")
deploymentName="deploymentscript-"$today

az deployment group create --resource-group $resourceGroupName --name $deploymentName --template-file $templateFile --parameters $templateParameterFile
```

テンプレートの結果確認

```shell
storageAccountName=$(az deployment group show --resource-group $resourceGroupName --name $deploymentName --query 'properties.outputs.storageAccountName.value' --output tsv)
az storage blob list --account-name $storageAccountName --container-name config --query '[].name'
```

```shell
az deployment-scripts show-log --resource-group $resourceGroupName --name CopyConfigScript
```

クリーンアップ

```shell
az group delete --name $resourceGroupName
```
