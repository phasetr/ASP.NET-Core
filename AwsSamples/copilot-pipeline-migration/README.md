# README

- **これを動かす場合はこのディレクトリだけ切り出して別途(`GitHub`に)リポジトリを作ること**!
- [AWS Copilot でデプロイ時にマイグレーションを実行する](https://qiita.com/grohiro/items/1beac175b4e85d38349b)
- CI/CDでテストも追加した
- `copilot`で生成したデータベースに対して`CDK`で`Bastion`を構成した

## memo

```shell
copilot task run --generate-cmd pipeline-migration/stg/pipeline-migration-svc
```

```shell
export AWS_PROFILE=dev
APP_NAME=pipeline-migration
env=stg
SVC_NAME=pipeline-migration-svc
cmd="$(copilot task run --generate-cmd ${APP_NAME}/${env}/${SVC_NAME} 2>&1 | \
  sed -E \
    -e 's/--entrypoint ""/--entrypoint \/app\/bundle/' \
    -e 's/--command ""//' \
  ) --follow --task-group-name pipeline-migration-release-${env}"
echo $cmd
```

```shell
eval "$cmd"
```

- さらにコマンドのテスト

```shell
export AWS_PROFILE=dev
pl_envs=("stg" "prod")
for env in $pl_envs; do
  echo ${env}
  echo pipeline-migration/${env}/pipeline-migration-svc
  cmd="$(copilot task run --generate-cmd pipeline-migration/${env}/pipeline-migration-svc 2>&1 | sed -E -e 's/--entrypoint ""/--entrypoint \/app\/bundle/' -e 's/--command ""//' -e 's/copilot /.\/copilot-linux /') --follow --task-group-name pipeline-migration-release-${env}"
  echo ${cmd}
done;
```

## コマンド

```shell
git add -A
git commit -m "first commit"
git branch -M main
git push -u origin main
```

```shell
export AWS_PROFILE=dev
aws whoami
```

```shell
copilot app init pipeline-migration
```

```shell
copilot env init --name stg \
  --profile dev \
  --app pipeline-migration \
  --default-config
```

```shell
copilot svc init --name pipeline-migration-svc \
  --svc-type "Load Balanced Web Service" \
  --dockerfile WebApi/WebApi/Dockerfile \
  --port 80
```

- `yaml`でヘルスチェックの記述を追加する

```shell
copilot storage init -n pipeline-migration-cluster \
  -l workload \
  -t Aurora \
  -w pipeline-migration-svc \
  --engine PostgreSQL \
  --initial-db mydb
```

- `job`からアクセスできるように`yaml`に記述を追加する
- `Program.cs`で`connectionString`を`secrets`から取得するようにする

```shell
copilot env deploy --name stg
copilot svc deploy --env stg
```

```shell
copilot job init -n dbmigrate \
  --dockerfile ./WebApi/WebApi/Dockerfile
```

- `job`の`yml`のデータベースの接続文字列関係の設定を書き換える：特に`secrets`を設定する。

```shell
copilot job deploy --name dbmigrate --env stg
copilot job run --name dbmigrate --env stg
```

- `job`の実行確認

```shell
copilot job logs --name dbmigrate --env stg
```

- ブランチのURLは適切に設定する

```shell
copilot pipeline init \
  --name copilot-pipeline-migration-main \
  --url https://github.com/phasetr/copilot-pipeline-migration \
  --git-branch main \
  --environments "stg" \
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

- うまくいかない場合の注意
  - `The "copilot" directory is not associated with an application.`：`.workspace`をコミットできているか確認

## 環境追加

```shell
copilot env init --name prod \
  --profile dev \
  --app pipeline-migration \
  --default-config
copilot env deploy --name prod && copilot svc deploy --env prod  
```

- `pipeline`の`yaml`で`stages`に`prod`を追加する
- `copilot svc show`の結果から`dbmigrate`の`yml`にデータベースの`SECRETS`を設定する

```shell
copilot pipeline deploy --yes
```

- 全体をコミット・プッシュする

## `copilot svc exec`による直接マイグレーション確認

```shell
copilot svc exec --env stg
./bundle
```

## Bastion

`cdk-bastion`ディレクトリ内の`README.md`を参照。

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
