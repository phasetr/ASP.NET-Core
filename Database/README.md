# README

- `docker-compose.yml`: データベースだけのファイル
- `docker-compose.with-dotnet.yml`: データベースに加えて`.NET`の開発用コンテナも含む
- `A5SQL`を使って作ったER図`doc/mydba5er`からSQLを生成して`db/init/init.sql`に置く
- SQLで`ID`を`Id`に置換する
- データベースを初期化：dockerを立ち上げれば自動的に初期化される
    - 再作成したい場合は`docker compose down`してから`docker compose up --build`
- スキャフォールドで`Product`のコントローラーだけ作成
- `TODO`：リレーションがきちんと張れているか？他のモデルに対する挙動も要調査。 

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
- cf. [スキャフォールドのオプション](https://learn.microsoft.com/ja-jp/ef/core/cli/dotnet#dotnet-ef-dbcontext-scaffold)

```shell
dotnet dotnet-ef dbcontext scaffold 'Host=localhost;Database=mydb;Username=user;Password=pass' Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context-dir Context --context MyDbContext
```

### コントローラーのスキャフォールド

- 今回は`Rider`のコントローラースキャフォールドで対応
- 実質的には`dotnet-aspnet-codegenerator`を発行している