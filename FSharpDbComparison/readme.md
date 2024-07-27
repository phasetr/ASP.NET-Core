# README

## データベース比較: 2024-07-27

Wlaschinの[関数型ドメインモデリング](https://tatsu-zine.com/books/domain-modeling-made-functional)いわく,
`F#`では`ORM`はあまり使わないらしい.
一方で[Dapper.FSharp](https://github.com/Dzoukr/Dapper.FSharp)は有名で時々耳に見かける.
マイグレーションに関しては`F#`の`Slack`で聞いたところ,
以下のような情報を得た.

ここでは`2024-07-27`までに`Slack`で教えて頂いた情報や,
`ORM`や型プロバイダによるデータベースアクセスの書き心地の確認を記録する.

## `ORM`・型プロバイダー

### `EF Core`

`ef-core.fsx`参照.
(ソースは`ChatGPT`に生成してもらった.)
ついでに`1.tmp.db`として`sqlite`のデータベースと基本的なデータも生成している.

これをもう少し整備して他のライブラリを使う場合の基本データにする.

### `Dapper.FSharp`

### 型プロバイダー

## マイグレーションツール

### `SSDT database projects (.sqlproj) for years alongside our SAFE Stack apps`

- [Using grate with SSDT Database Projects](https://www.compositional-it.com/news-blog/using-grate-with-ssdt-database-projects/)
- [Migration-based database development](https://www.compositional-it.com/news-blog/migration-based-database-development/)

`SQL Server`専用ツールのようだ.
`SQL Server`はあまり使おうと思わないのだがどうするか,
というのが第一の感想.

### `grate`

上記記事中での著者お勧め.

- [grate](https://erikbra.github.io/grate/)
- 上記の記事中で`Dbup`よりお勧めされていた.
- これもプレーンSQLスクリプトを使うタイプ.
- プレーンでいくならこれでいいのでは?

### `Dbup`

- [Dbup](https://dbup.readthedocs.io/en/latest/)

`dotnet tool install`よりも`Nuget`を使ってライブラリとして入れて使うタイプ(?).
そうでなくても使えるようだが.
これもプレーンな`SQL`スクリプトを使うタイプ.

### `Evolve`

- [Evolve](https://evolve-db.netlify.app/)
- プレーンなSQLスクリプトを使うタイプ
