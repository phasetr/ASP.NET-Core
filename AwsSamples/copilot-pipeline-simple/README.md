# README

- `Pipeline`でコマンドを実行するだけのサンプル
- **これを動かす場合はこのディレクトリだけ切り出して別途(`GitHub`に)リポジトリを作ること**!
- 参考：[公式チュートリアル](https://aws.github.io/copilot-cli/ja/docs/concepts/pipelines/)
- 参考：[copilot pipeline manifest](https://aws.github.io/copilot-cli/ja/docs/manifest/pipeline/)

## 概要

- 先に`env`, `svc`を先に作って一通り動くようにした上で`pipeline`を試すようにしよう
  - `copilot deploy pipeline`してしまうと`copilot svc deploy`が走らなくなる模様

## コマンド

- `Git`のブランチが`main`であるか確認する

```shell
git branch
```

```shell
export AWS_PROFILE=dev
aws whoami
```

```shell
copilot app init pipeline-simple
```

```shell
copilot env init --name stg \
  --profile dev \
  --app pipeline-simple \
  --default-config
copilot env init --name prod \
  --profile dev \
  --app pipeline-simple \
  --default-config
```

```shell
copilot svc init --name pipeline-simple-svc \
  --svc-type "Load Balanced Web Service" \
  --dockerfile Dockerfile \
  --port 80
```

```shell
copilot env deploy --name stg && copilot env deploy --name prod
copilot svc deploy --env stg && copilot svc deploy --env prod
```

- ブランチのURLは適切に設定する

```shell
copilot pipeline init \
  --name copilot-pipeline-simple-main \
  --url https://github.com/phasetr/copilot-pipeline-simple \
  --git-branch main \
  --environments "stg,prod" \
  --pipeline-type "Workloads"
```

```shell
git add copilot/ && git commit -m "Adding pipeline artifacts" && git push
```

```shell
copilot pipeline deploy
```

- コマンド実行で出てきた`URL`を開く
- 「保留中」の項目のステータスが「利用可能」になるように`GitHub`で設定する
- 必要に応じて`pipeline`の`manifest.yml`を書き換える
- ファイルを全てコミットする

```shell
copilot pipeline deploy --yes
```

## 削除

```shell
export AWS_PROFILE=dev
copilot pipeline delete
copilot svc delete --env prod
copilot svc delete --env stg
copilot env delete --name prod
copilot env delete --name stg
copilot app delete
```
