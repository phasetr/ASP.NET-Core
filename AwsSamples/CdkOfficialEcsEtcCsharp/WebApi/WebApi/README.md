# README

- [AWS CDKでApp Runnerに実行時の環境変数を渡す](https://www.findxfine.com/infrastructure/aws/995562497.html)

## Initialize

```shell
dotnet tool restore
dotnet restore
```

## Dockerでの実行

```shell
# M1 Macだとarm64でビルドされるはず
docker build -t webapi --target final .
docker run -it --rm -p 80:80 webapi

# x86_64でビルドする
docker buildx build -t webapi --platform linux/amd64 --target final .
docker run -it --rm -p 80:80 webapi
```

## 手動でECRにアップロード

- `ECR`のコンソールにアクセス
- 適切なリポジトリを選択
- 画面上部の「プッシュコマンドの表示」を選択
- 案内に従ってコマンドを実行する
