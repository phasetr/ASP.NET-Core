# README

## Bicep

- [クイックスタート: Bicep を使用して新しい Azure API Management サービス インスタンスを作成する](https://learn.microsoft.com/ja-jp/azure/api-management/quickstart-bicep?tabs=CLI)

```shell
az group create --name exampleRG --location eastus
az deployment group create --resource-group exampleRG \
  --template-file main.bicep \
  --parameters publisherEmail=phasetr@gmail.com publisherName=phasetr
```

- デプロイされているリソースの確認

```shell
az resource list --resource-group exampleRG
```

- リソースをクリーンアップ

```shell
az group delete --name exampleRG
```

## CLI

- [クイック スタート: Azure CLI を使用して新しい Azure API Management サービス インスタンスを作成する](https://learn.microsoft.com/ja-jp/azure/api-management/get-started-create-service-instance-cli)
- 前提

```shell
az login
az version
az upgrade
```

- リソースグループ作成

```shell
az group create --name myResourceGroup --location centralus
```

- 新しいサービスの作成

```shell
az apim create --name ptapim --resource-group myResourceGroup --publisher-name phasetr --publisher-email phasetr@gmail.com --no-wait
```

- デプロイの状態を確認

```shell
az apim show --name ptapim --resource-group myResourceGroup --output table
```

- リソースのクリーンアップ

```shell
az group delete --name myResourceGroup
az apim delete --name ptapim
```
