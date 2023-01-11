# README

## 参考
- [自サイト](https://phasetr.com/archive/fc/pg/fsharp/#f)のASP.NETの記録を参考にすること.
- 各プロジェクトの初期化コマンドは次の通り.

```shell
dotnet tool restore
dotnet libman restore # 必要ないプロジェクトあり
dotnet restore
```

## 共通メモ
- いちいち起動するのが面倒だから基本は`docker`なし
- 断わりがなければデータベースは`SQLite`
- `docker-compose.yml`: データベースだけのファイル
- `docker-compose.with-dotnet.yml`: データベースに加えて`.NET`の開発用コンテナも含む

## 各ディレクトリの説明
- `TODO`
    - EF Core Identity
        - サービスのメインのテーブルがあるのと同じデータベースに作れるか?
        - ユーザーテーブルにリレーションを張る
- BlazorFluentUi
    - `dotnet new blazorwasm -o BlazorFluentUi`
    - [Use Fluent UI Web Components with Blazor](https://learn.microsoft.com/ja-jp/fluent-ui/web-components/integrations/blazor)
    - [ビジュアルでの一覧](https://brave-cliff-0c0c93310.azurestaticapps.net/)
    - [Web components overview](https://learn.microsoft.com/en-us/fluent-ui/web-components/components/overview)
    - [Fluent Icons](https://fluenticons.co/)
    - [Fast](https://www.fast.design/)
    - [UIフレームワーク調査](https://blazor-master.com/blazor-ui-framework/)
        - [MatBlazor](https://www.matblazor.com/)
        - [Ant Design Blazor](https://antblazor.com/en-US/)
        - [Radzen](https://blazor.radzen.com/)
        - [Syncfusion](https://www.syncfusion.com/blazor-components)
        - [Skclusive UI](https://skclusive.github.io/Skclusive.Material.Docs/)
- BlazorWasmTodo
    - [Blazor Todo リスト アプリを構築する](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/tutorials/build-a-blazor-app?view=aspnetcore-6.0&pivots=webassembly)
    - `dotnet new blazorwasm -o BlazorWasmTodo`
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
- IdentityByController
    - コントローラーを使った認証
    - オリジナルは`SportsStore`で, 認証以外をほぼ削除: `Product`だけ残してある
    - データベースは`SQLite`
- IdentityByRazorPages
    - `RazorPages`での認証
    - オリジナルは[Pro ASP.NET Core 6 ](https://github.com/Apress/pro-asp.net-core-6/tree/main/39%20-%20ASP.NET%20Core%20Identity%20-%202/End%20of%20Chapter)
    - データベースは`SQLite`
- MvcWithApi
    - MVCの公式チュートリアルが大元
        - [公式](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-6.0&tabs=visual-studio)
        - [ASP.NET Core 6 MVCのチュートリアルを優しく解説](https://masa7blog.com/asp-net-core-6-mvc-tutorial/)
    - MVCとAPIを同居させたサンプルプロジェクト
    - APIは下記`SimpleWebApi`と同じく[チュートリアル](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio)から
    - データベースは`SQLite`
- RazorPages
    - [チュートリアル: ASP.NET Core の Razor Pages の概要](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-6.0&tabs=visual-studio-code)
    - `MvcWithApi`の`MVC`チュートリアル部分を`Razor Pages`に変えただけ
    - ASP.NET Coreからはふつうの`MVC`よりこちらが推奨とのこと
    - `TODO`: テストも書ける
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
- 認証, API認証(JWT)
    - [ASP.NET Core Identityと既存テーブルの連携(?)](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-7.0)
    単純に[この](https://gavilan.blog/2018/04/15/relationship-between-tables-and-aspnetusers/)外部キー配置でいける?
- ファイルアップロード
- メール送信
- ログのファイルへの書き出し
- [Blazor公式](https://dotnet.microsoft.com/ja-jp/apps/aspnet/web-apps/blazor)
- `GlobalUsing.cs`
- ワンタイムURL
- テスト
    - [コンテナ活用](https://github.com/testcontainers/testcontainers-dotnet)
    - [Running Tests with Docker](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
- [Giraffe](https://github.com/giraffe-fsharp/Giraffe)/[Saturn](https://saturnframework.org/)
    - [Giraffe samples](https://github.com/giraffe-fsharp/samples)
