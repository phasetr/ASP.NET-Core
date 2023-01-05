# README

- HelloDockerWeb: `docker compose`を使った最小サンプル
    - `http://localhost/`にアクセスすると`Hello Docker!`と表示されるだけ
- MvcMovie: MVCの公式チュートリアル
    - `docker`で`PostgreSQL`利用
    - TODO `docker compose`に`.NET`追加
- SimpleTest: モデルクラスが一つだけあるテスト.
  データソースが`DI`で抽象化されたコントローラーをテストしている.
- SimpleWebApi: インメモリデータベースを使ったシンプルなAPIのサンプル
    - `.NET`用の`docker compose`あり
- SportsStore: [Pro ASP.NET Core 6, 2022](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205)のサンプルコード
    - `libman`利用
    - `docker`で`PostgreSQL`利用
    - TODO `docker compose`に`.NET`追加

## TODO
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
