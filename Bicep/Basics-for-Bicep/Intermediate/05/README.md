# README

- [What-If を使用して Azure デプロイの変更をプレビューする](https://learn.microsoft.com/ja-jp/training/modules/arm-template-whatif/)

- `az deployment group what-if`

```shell
az deployment group what-if \
  --resource-group ToyStorage \
  --template-file $templateFile \
  --result-format FullResourcePayloads
```

- サンドボックスをつける：画面が狭いとONにするための表示が出てこない可能性があるため、ブラウザは大きな画面幅で見ること

```shell
az login --allow-no-subscriptions
```

```shell
az account set --subscription "Concierge Subscription"
```

```shell
az account list \
  --refresh \
  --query "[?contains(name, 'Concierge Subscription')].id" \
  --output json \
  | jq . '.[]' | az account set --subscription
```

既定のリソースグループを設定

```shell
az configure --defaults group="learn-8007e136-1e76-469f-8347-415318259daa"
```

適切なディレクトリ移動しているか確認する

```shell
az deployment group create --template-file main.bicep
```

画面の「[デプロイを検証する](https://learn.microsoft.com/ja-jp/training/modules/arm-template-whatif/4-exercise-what-if?tabs=screenshpt&pivots=bicepcli)」の項目を見て、デプロイを確認する。

画面に沿って`main.bicep`を編集。

```shell
az deployment group what-if --template-file main.bicep
```

`main.bicep`の内容を全削除する。

```shell
az deployment group create \
  --mode Complete \
  --confirm-with-what-if \
  --template-file main.bicep
```
