# README

## 事前確認

- ここで指定済みの`copilot`ディレクトリの内容で再生成するときは`copilot/web/addons/iam-secrets-manager-policy.yml`のデータベース接続用の`WEBCLUSTER_SECRET`と`WEBCLUSTER_SECRET_ARN`の値は適切に書き換えること。

## command

```shell
# build
docker compose build -f compose.build.yml build
# 実行
docker compose -f compose-with-dotnet.yml up
# データベースだけ立ち上げる
docker compose up
```

## Connection String

### SQLite

```
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  },
```

### PostgreSQL

```
  "ConnectionStrings": {
    "DefaultConnection": "User ID=user;Password=pass;Host=localhost;Port=5432;Database=mydb;"
  },
```

## commands

```shell
dotnet new tool-manifest
dotnet tool install dotnet-ef --version 6.0.14
```

```shell
dotnet add package Microsoft.EntityFrameworkCore --version 6.0.14
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 6.0.8
```

```shell
dotnet dotnet-ef migrations add AddUser
dotnet dotnet-ef database update
```
