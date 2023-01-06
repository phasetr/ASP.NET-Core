# README

- `docker-compose.yml`: データベースだけのファイル
- `docker-compose.with-dotnet.yml`: データベースに加えて`.NET`の開発用コンテナも含む
- `A5SQL`を使って作ったER図`doc/mydba5er`からSQLを生成して`db/init/init.sql`に置いてこれでデータベースを初期化

## 初期化
### ツールのインストール

```shell
dotnet new tool-manifest
dotnet tool install dotnet-ef --version 6.0.12
```

### パッケージのインストール

```shell
dotnet add package Microsoft.EntityFrameworkCore --version 6.0.12
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.12
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 6.0.8
```

### データベースのスキャフォールド
- `TODO`：接続文字列のセキュリティに関連して[シークレットマネージャーツール](https://learn.microsoft.com/ja-jp/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows#secret-manager)を使うべしと怒られる

```shell
dotnet dotnet-ef dbcontext scaffold 'Data Source="localhost, 5432";Initial Catalog=mydb;User ID=user;Password=pass' Npqsql.EntityFrameworkCore.PostgreSQL
dotnet dotnet-ef dbcontext scaffold 'Host=localhost;Database=mydb;Username=user;Password=pass' Npgsql.EntityFrameworkCore.PostgreSQL
```
