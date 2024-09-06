# README

- ラーニングパス: [Azure Developer CLI を使用してアプリケーションを構築してデプロイする](https://learn.microsoft.com/ja-jp/training/paths/azure-developer-cli/?source=recommendations)
- フロント・バックエンドともに`node.js`

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

環境名の入力を要求されたら`azdlearn`を指定。

```shell
azd auth login
azd up
```

Azure portalで`rg-azdlearn`でリソースグループを調べる。

アプリケーションの監視

```shell
azd monitor --live
```

- `azd deploy`：このコマンドではアプリケーションのソース コードがパッケージ化され、Azure に再デプロイされます。
  ただし、Azure リソースに変更を適用するコード ファイルとしてのインフラストラクチャは実行されません。
- `azd provision`：このコマンドではコードファイルとしてのインフラストラクチャへの変更に基づいて`Azure`リソースが作成または更新されます。 
  たとえば`Bicep`または`Terraform`ファイルに追加した場合、`azd provision`を実行すると`Azure`に新しいストレージ アカウントが作成されます。 
  ただし、このコマンドによってアプリケーションのソース コードがパッケージ化されたり再デプロイされたりすることはありません。

[テンプレートに更新を適用する](https://learn.microsoft.com/ja-jp/training/modules/deploy-configure-azure-developer-cli-template/4-update-template)に沿ってファイルを編集。

```shell
azd deploy
```

同じく適切なファイルを編集。

```shell
azd provision
```

## テンプレートの CI/CD パイプラインを構成する

- <https://learn.microsoft.com/ja-jp/training/modules/deploy-configure-azure-developer-cli-template/5-configure-pipeline>

省略。

```shell
azd pipeline config
```

## リソースをクリーンアップする

- <https://learn.microsoft.com/ja-jp/training/modules/deploy-configure-azure-developer-cli-template/6-clean-up-resources>

```shell
azd down
```
