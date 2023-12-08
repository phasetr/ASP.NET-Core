# README

- [Tooling for ASP.NET Core Blazor](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/tooling?view=aspnetcore-8.0&pivots=linux-macos)
- [クイックスタート: ASP.NET Web アプリをデプロイする](https://learn.microsoft.com/ja-jp/azure/app-service/quickstart-dotnetcore?tabs=net70&pivots=development-environment-cli)

## プロジェクトの初期化

```shell
dotnet new blazor -o CliBlazor
```

## リリース

```shell
az login
```

- 次のコマンドで適切なグループを調べる

```shell
az group list | jq 'map({name,location})'
```

- 適切なグループのリソースグループ名とロケーションを取得する

```shell
resourceGroupName=sample01
location=$(az group list | jq -r '.[] | select(.name == "sample01") | .location')
```

- 変数に入ったか確認する

```shell
echo ${resourceGroupName}
echo ${location}
```

```shell
az webapp up -g ${resourceGroupName} -l ${location} --sku F1 --name MyUniqueAppName01 --os-type linux
```

- 再アップロード

```shell
az webapp up -g ${resourceGroupName} -l ${location} --name MyUniqueAppName01 --os-type linux
```

## 削除

```shell
az webapp delete -g ${resourceGroupName} -n MyUniqueAppName01
```

- 消去を確認

```shell
az webapp list
```
