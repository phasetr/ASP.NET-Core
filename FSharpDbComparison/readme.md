# README

## データベース比較: 2024-07-27

Wlaschinの[関数型ドメインモデリング](https://tatsu-zine.com/books/domain-modeling-made-functional)いわく,
`F#`では`ORM`はあまり使わないらしい.
一方で[Dapper.FSharp](https://github.com/Dzoukr/Dapper.FSharp)は有名で時々耳に見かける.
マイグレーションに関しては`F#`の`Slack`で聞いたところ,
以下のような情報を得つつ調査した.

ここでは`2024-07-27`までに`Slack`で教えて頂いた情報や,
`ORM`や型プロバイダによるデータベースアクセスの書き心地の確認を記録する.

## `ORM`

- `Compositional IT`のまとめ記事: [SQL series wrap up](https://www.compositional-it.com/news-blog/sql-series-wrap-up/)

### `Dapper.FSharp`

想像以上にはまってしまった.
`Rider`で見ているとコードに色がついている部分があって問題が解決できない.
とりあえず比較用の簡易実装としてはこのままで詳しくわかったら追記したい.

### `EF Core`

- [EFCore.FSharp](https://efcore.github.io/EFCore.FSharp/)
- GitHub, [GETTING_STARTED.md](https://github.com/efcore/EFCore.FSharp/blob/master/GETTING_STARTED.md)

データベースは界面の外側として,
マイグレーションまで含めて完全に`EF Core`に任せる手はないではない.
`F#`ではなく`C#`にしてしまう方向性さえある.
`EF Core`は`Identity`もある.

`EFCore.FSharp`では[RecordHelper](https://efcore.github.io/EFCore.FSharp//How_Tos/Use_DbContextHelpers.html)を使うとよい.

#### 参考：`C#`での`EF Core`

`EFCoreCSharp`のような処理を`EFCore.FSharp`で書きたい.

#### 参考: `F#`での単純な`EF Core`（`EFCore.FSharp`ではない）

`efcore.fsx`参照.
(ソースは`ChatGPT`に生成してもらった.)
`sqlite`のデータベースと基本的なデータも生成している.
`F#`レベルで`Fluent API`で複合キーやリレーションを設定しようとしたらうまくいかなかったため,
いったんごく基本的なサンプルだけで断念.

### `SqlProvider`

- [GitHub](https://github.com/fsprojects/SQLProvider/)
- [ドキュメントサイト](https://fsprojects.github.io/SQLProvider/core/general.html)

>SQL Server、Access、ODBC 以外のデータベース ベンダーを使用する場合は、サード パーティ ドライバーが必要です。

とりあえず`Mac`+`fsx`での利用例を考える.

`SQLite`だと`dll`の指定が必要らしく,
果てしなく面倒で`SQLite`でのコード検証は断念.
`PostgreSQL`や`MySQL`だともう少し楽なのかもしれないが.

もう一つ懸念点がある.
[この記事](https://www.compositional-it.com/news-blog/full-orms-and-f/)で次のようにある.

>プロバイダーは常にデータベースへのライブ接続を必要とするため, `CI`などの開発のさまざまな段階で注意が必要.

ただし解決策に関して次の記事への参照がある.

- [Structuring an F# project with SQL Type Provider on CI](https://medium.com/datarisk-io/structuring-an-f-project-with-sql-type-provider-on-ci-787a79d78699)

#### `MariaDB`での利用

`sqlprovider-mariadb.fsx`参照.
`compose-mariadb.yml`で`MariaDB`を立ち上げておくこと.
スクリプト中で`MySqlConnector.dll`をコピーしてきてそれを読み込む形にしている.

## マイグレーションツール

### `SSDT database projects (.sqlproj) for years alongside our SAFE Stack apps`

- [Using grate with SSDT Database Projects](https://www.compositional-it.com/news-blog/using-grate-with-ssdt-database-projects/)
- [Migration-based database development](https://www.compositional-it.com/news-blog/migration-based-database-development/)

`SQL Server`専用ツールのようだ.
`SQL Server`はあまり使おうと思わないのだがどうするか,
というのが第一の感想.

### `grate`

上記記事中での著者のお勧め.

- [grate](https://erikbra.github.io/grate/)
- 上記の記事中で`Dbup`よりお勧めされていた.
- これもプレーンSQLスクリプトを使うタイプ.
- プレーンでいくならこれでいいのでは?
- `dotnet tool install --glocal grate`(または`--local`)でインストール
- 基本的には`up`, `down`などが置いてあるディレクトリ内で`grate`コマンドを実行
  - ここでは`grate`ディレクトリに必要なファイルをまとめていて,
    `-f`オプションでルートディレクトリが指定できる.
  - `.envrc`に`MariaDB`の`compose.yml`に指定した情報からの接続文字列を指定した.
  - 執筆時点で`MySQL`だとエラーが出たため`MariaDB`を利用している.
- [コマンドオプションの説明ページ](https://erikbra.github.io/grate/configuration-options/)
  - `-o`: 移行に関連するすべてのものが保存される場所.
    すべてのバックアップ・実行されたすべてのアイテム・権限ダンプ・ログなど.
    - `–env`: 適用したい環境指定

#### `grate`の（ローカル）インストール

- 以下のコマンドで`grate`をインストール

```shell
dotnet new tool-manifest
dotnet tool install --local grate
```

#### `grate-sqlite`用実行メモ

- マイグレーション実行

```shell
dotnet tool run grate \
  -c="Data Source=grate-sqlite.db" \
  -f grate-sqlite \
  --dbt sqlite
```

#### TODO `grate-sqlserver`実行用メモ

- **まだ成功していない.**
- `Mac`では` Microsoft.Data.SqlClient`が標準では欠けていてあまり嬉しくない.
- `Docker`を立ち上げる

```shell
docker compose -f compose-sqlserver.yml up
```

- 未確認：`grate-sqlserver/init-db`にある`SQL`でデータベースを作っているつもりだがうまくいっていないかもしれない.
  `Rider`からの接続など適当な手段で`Database=GrateSample`を作ること.
- マイグレーション実行

```shell
dotnet tool run grate \
  -c="Server=localhost,1433;Database=mydb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True" \
  -f grate-sqlserver \
  --dbt sqlserver
```

- `Docker`を落とす

```shell
docker compose -f compose-sqlserver.yml down
```

#### `grate-mariadb`実行用メモ

- **これは成功**
- `Docker`を立ち上げる.

```shell
docker compose -f compose-mariadb.yml up
```

- 接続文字列：`Server=localhost;Port=3306;Database=mydb;User Id=user;Password=pass;`
- マイグレーション実行

```shell
dotnet tool run grate \
  -c="Server=localhost;Port=3306;Database=mydb;User Id=user;Password=pass;" \
  -f grate-mariadb \
  --dbt mariadb
```

- `Docker`を落とす

```shell
docker compose -f compose-mariadb.yml down
```

### `Dbup`

- [Dbup](https://dbup.readthedocs.io/en/latest/)

`dotnet tool install`よりも`Nuget`を使ってライブラリとして入れて使うタイプ(?).
そうでなくても使えるようだが.
これもプレーンな`SQL`スクリプトを使うタイプ.
関係するコードは書いていない.

### `Evolve`

- [Evolve](https://evolve-db.netlify.app/)
- プレーンなSQLスクリプトを使うタイプ

これも関係するコードは書いていない.

### `FluentMigrator`

- [Fluent migrations framework for .NET](https://fluentmigrator.github.io/)

>Fluent Migrator is a migration framework for .NET much like Ruby on Rails Migrations.

実際に`C#`のコードを書いて管理する.
`Slack`で聞いた限りダウングレードにも良く対応しているとのこと.
