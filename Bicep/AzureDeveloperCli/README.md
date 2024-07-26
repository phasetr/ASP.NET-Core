# README

- ラーニングパス: [Azure Developer CLI を使用してアプリケーションを構築してデプロイする](https://learn.microsoft.com/ja-jp/training/paths/azure-developer-cli/?source=recommendations)

## インストール

```shell
brew tap azure/azd && brew install azd
```

インストール確認

```shell
azd version
```

- [awesome-azd](https://azure.github.io/awesome-azd/)

コマンド例

```text
azd init --template todo-nodejs-mongo
azd auth login
azd up
```

テンプレート一覧の確認

```shell
azd template list
```

テンプレートのデプロイ

```shell
azd init --template todo-nodejs-mongo
```
