# README

- A Developer's Guide to .NET in Azure, Chapter2

## 注意

うまくいかなかったため本の`packtbook`を`phasetr`に変更.
統一感を持たせるため, 全体的にコマンドに`packt`, `packtbook`とあったら`packt`を`phasetr`に変更.
実験時, `Azure DevOps`での`CI/CD`設定がうまくいかなかったが,
一通りコマンドを打って何となくうまくいったので初回実験としては良しとした.
後半のコマンドではいくつか廃止予定のオプションなどを使っていたためそれを適切に書き換える必要がある.

`ASP.NET`としては`.NET 6.0`で`minimal api`による`API`プロジェクトか`Razor Pages`になっている.

最後にリソースグループを削除するコマンドもつけてある.

## memo

```shell
dotnet new web -o Chapter02 --framework net6.0
```

```shell
dotnet publish --configuration Release --output published
```

```shell
docker build -t phasetr.azurecr.io/chapter02:1.0.0 .
```

```shell
docker run --publish 5000:80 phasetr.azurecr.io/chapter02:1.0.0
```

## Azure Container Registry の作成と構成

```shell
az version
```

```shell
az login
```

```shell
az group create --name chapter2 --location centralindia
```

```shell
az acr create --resource-group chapter2 --name phasetr --sku Basic
```

```shell
az acr login --name phasetr
```

```shell
docker build -t phasetr.azurecr.io/chapter2:1.0.0 .
```

```shell
docker tag hello-world:latest phasetr.azurecr.io/ hello-world:latest
```

```shell
docker push phasetr.azurecr.io/chapter2:1.0.0
```

```shell
az acr repository list --name phasetr --output table
```

```shell
docker rmi phasetr.azurecr.io/chapter2:1.0.0
```

```shell
docker pull phasetr.azurecr.io/chapter2:1.0.0
docker run --publish 5001:80 phasetr.azurecr.io/chapter2:1.0.0
```

## Azure Container Registry から Azure App Service への Web アプリのデプロイ

```shell
az acr update -n phasetr --resource-group chapter2 --admin-enabled true
```

```shell
az acr credential show --name phasetr --resource-group chapter2
password=$(az acr credential show --name phasetr --resource-group chapter2 | jq -r '.passwords[0].value')
echo $password
```

```shell
az group create --name chapter2-example --location centralus
az appservice plan create --name chapter2-app-plan --resource-group chapter2-example --location centralus --is-linux --sku B1
```

```shell
# obsolete：次のコードブロックを利用すること
az webapp create --name phasetr-app-service --plan chapter2-app-plan --resource-group chapter2-example --deployment-container-image-name phasetr.azurecr.io/chapter2:1.0.0
```

```shell
# Webアプリケーションの作成
az webapp create --name phasetr-app-service --plan chapter2-app-plan --resource-group chapter2-example
# コンテナイメージの設定
az webapp config container set --name phasetr-app-service --resource-group chapter2-example --container-image-name phasetr.azurecr.io/chapter2:1.0.0
```

<https://phasetr-app-service.azurewebsites.net/>にアクセス。

```shell
password=$(az acr credential show --name phasetr --resource-group chapter2 | jq -r '.passwords[0].value')
echo $password
az webapp create --name phasetr-app-service --plan chapter2-app-plan --resource-group chapter2-example --deployment-container-image-name phasetr.azurecr.io/chapter2:1.0.0 --docker-registry-server-password ${password} --docker-registry-server-user phasetr

az webapp config container set --name phasetr-app-service --resource-group chapter2-example --container-image-name phasetr.azurecr.io/chapter2:1.0.0 --container-registry-url https://phasetr.azurecr.io --container-registry-user phasetr --container-registry-password ${password}
```

```shell
az acr repository list --name phasetr --output table
```

```shell
az identity create --name chapter2-identity --resource-group chapter2
```

```shell
principalId=$(az identity show --resource-group chapter2 --name chapter2-identity --query principalId --output tsv)
echo ${principalId}
```

```shell
registryId=$(az acr show --resource-group chapter2 --name phasetr --query id --output tsv)
echo ${registryId}
```

```shell
az role assignment create --assignee $principalId --scope $registryId --role "AcrPull"
```

```shell
az appservice plan create --name chapter2-app-plan --resource-group chapter2-example --location centralus --is-linux --sku B1
az webapp create --name phasetr-app-service --plan chapter2-app-plan --resource-group chapter2-example --deployment-container-image-name phasetr.azurecr.io/chapter2:latest
```

```shell
az webapp config appsettings set --resource-group chapter2-example --name phasetr-app-service --settings WEBSITES_PORT=5069
```

```shell
id=$(az identity show --resource-group chapter2 --name chapter2-identity --query id --output tsv)
echo ${id}
az webapp identity assign --resource-group chapter2-example --name phasetr-app-service --identities $id
```

```shell
appConfig=$(az webapp config show --resource-group chapter2-example --name phasetr-app-service --query id --output tsv)
az resource update --ids $appConfig --set properties.acrUseManagedIdentityCreds=True
```

```shell
clientId=$(az identity show --resource-group chapter2 --name chapter2-identity --query clientId --output tsv)
az resource update --ids $appConfig --set properties.AcrUserManagedIdentityID=$clientId
```

```shell
cicdUrl=$(az webapp deployment container config --enable-cd true --name phasetr-app-service --resource-group chapter2-example --query CI_CD_URL --output tsv)
echo ${cicdUrl}
az acr webhook create --name phasetrcd --resource-group chapter2 --registry phasetr --uri $cicdUrl --actions push --scope chapter2:latest
```

```shell
az group delete --name chapter2 --yes --no-wait
az group delete --name chapter2-example --yes --no-wait
az group list --output table
```
