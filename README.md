# README

## 参考
- [自サイト](https://phasetr.com/archive/fc/pg/fsharp/#f)のASP.NETの記録を参考にすること.

## 共通メモ
- いちいち起動するのが面倒だから基本は`docker`なし
- 断わりがなければデータベースは`SQLite`
- `docker-compose.yml`: データベースだけのファイル
- `docker-compose.with-dotnet.yml`: データベースに加えて`.NET`の開発用コンテナも含む

## 各ディレクトリの説明
- `TODO` EFCore
    - `Entity Framework Core`の実験場.
    - 壊しやすいようにデータベースは`SQLite`
- Database
    - 先にデータベースを作ってから`EF Core`でリバースエンジニアリング
    - `docker`で`PostgreSQL`利用
    - ついでにコントローラーをスキャフォールド
- HelloDockerWeb
    - `docker compose`を使った最小サンプル
    - データベースなし
    - `http://localhost/`にアクセスすると`Hello Docker!`と表示されるだけ
- MvcMovie
    - `docker`で`PostgreSQL`利用
- MvcWithApi
    - MVCの公式チュートリアル
    - MVCとAPIを同居させたサンプルプロジェクト
    - データベースは`SQLite`
- SimpleTest
    - モデルクラスが一つだけあるテストを書いたプロジェクト
    - データベース未使用
    - データソースが`DI`で抽象化されたコントローラーをテストしている
- SimpleWebApi
    - インメモリデータベースを使ったシンプルなAPIのサンプル
- SportsStore: [Pro ASP.NET Core 6, 2022](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205)のサンプルコード
    - ある程度の規模があるサンプルプロジェクト
    - `docker`で`PostgreSQL`利用

## TODO
- データベースのリレーション
- 認証, API認証(JWT)
- ログのファイルへの書き出し
- `GlobalUsing.cs`
- テスト
    - [コンテナ活用](https://github.com/testcontainers/testcontainers-dotnet)
    - [Running Tests with Docker](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
- Fluent UI, [UIフレームワーク調査](https://blazor-master.com/blazor-ui-framework/), [Fast](https://www.fast.design/)
- MVC+API
- Blazor: Server, WebAssembly
- [Giraffe](https://github.com/giraffe-fsharp/Giraffe)/Saturn
    - [Giraffe samples](https://github.com/giraffe-fsharp/samples)
