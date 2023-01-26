# README

## 初期化
- 初期化に関して[自サイト](https://phasetr.com/archive/fc/pg/fsharp/#f)の`ASP.NET`の記録も参考にすること.
- 各プロジェクトの初期化コマンドは次の通り.

```shell
dotnet tool restore
dotnet libman restore # 必要ないプロジェクトあり
dotnet restore
dotnet dotnet-ef migrations add Init # Migrationsをコミットしていない場合は初期化
dotnet dotnet-ef database update # SQLiteの初期化
```

## 共通メモ
- いちいち起動するのが面倒だから基本は`docker`なし
- 断わりがなければデータベースは`SQLite`
- `docker-compose.yml`: データベースだけのファイル
- `docker-compose.with-dotnet.yml`: データベースに加えて`.NET`の開発用コンテナも含む

## 参考資料

### 認証
- [URL](https://zenn.dev/okazuki/articles/add-auth-to-blazor-server-app)
    - `@attribute [Authorize(Roles = "Administrator")]`をつけるとロールで認可振り分けできる
### ページャー
#### 簡易自前実装
- [2018 Simple Paging In ASP.NET Core Razor Pages](https://www.mikesdotnetting.com/article/328/simple-paging-in-asp-net-core-razor-pages)
#### ライブラリ: LazZiya/TagHelpers
- [GitHub](https://github.com/LazZiya/TagHelpers)
- [解説](https://ziyad.info/en/articles/21-Paging_TagHelper_for_ASP_NET_Core)

## 各ディレクトリの説明
### BlazorFluentUi
- `dotnet new blazorwasm -o BlazorFluentUi`
- `DB`: なし
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
### BlazorWasmTodo
- `DB`: なし
- [Blazor Todo リスト アプリを構築する](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/tutorials/build-a-blazor-app?view=aspnetcore-6.0&pivots=webassembly)
- `dotnet new blazorwasm -o BlazorWasmTodo`
### CityBreaks
- [ASP.NET Core Razor Pages In Action](https://github.com/mikebrind/Razor-Pages-In-Action)
- Chapter14, AppSettingsStronglyTypedPOCO
- オリジナルのリポジトリにあるデータベースには次のアカウントがある
    - `anna@test.com`/`password`
- アクセスできる`URL`参照: `Program.cs`に設定がある
    - `property-manager`などキャメルケースをケバブケースに変える
- `Ajax`参考
    - ユーザーとしてアクセスしてどれかの街を選ぶ
    - ホテル名をクリックするとモーダルが出る: このモーダルで`Ajax`を利用している
    - `Pages/City.cshtml`内でモーダルを呼んでいる
    - `handler=propertydetails`は`cshtml.cs`の`OnGetPropertyDetails`メソッドを呼んでいる
    - `SHared`の`_PropertyModalPartial.cshtml`と`_PropertyDetailsPartial.cshtml`がモーダル処理
    - `API`は`Minimal API`として`Program.cs`内でアクセスポイントを作っている
- `Filters/AsyncPageFilter.cs`に`IAsyncPageFilter`を実装してアクセスログを実装
    - `Program.cs`の`builder.Services.AddRazorPages.AddMvcOptions`でフィルターを仕込んでいる.
    - 参考: [モデルバインド前後にログを仕込む](https://qiita.com/gushwell/items/bcf39aaf708b9a483cf5),
      [自サイト](https://phasetr.com/archive/fc/pg/dotnet/aspdotnet/)にもメモ.
- 例外処理をフィルターに追加
### Database
- `DB`: `docker`で`PostgreSQL`利用
- 先にデータベースを作ってから`EF Core`でリバースエンジニアリング
- ついでにコントローラーをスキャフォールド
### EfCoreRazorPages
- `DB`: `SQLite`
- `Entity Framework Core`の実験場.
- `Identity`のユーザーテーブルにリレーションを張る: [EF Core 認証用ユーザーにリレーションを張る](https://phasetr.com/archive/fc/pg/fsharp/)
- cf. [`Razor Pages`なしでの直接認証は不可能](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-7.0&tabs=netcore-cli#style-authentication-endpoints)
### HelloDockerWeb
- `DB`: なし
- `docker compose`を使った最小サンプル
- `http://localhost/`にアクセスすると`Hello Docker!`と表示されるだけ
### IdentityByController
- `DB`: `SQLite`
- コントローラーを使った認証
- オリジナルは`SportsSln`で, 認証以外をほぼ削除: `Product`だけ残してある
### IdentityByRazorPages
- `DB`: `SQLite`
- `RazorPages`での認証, `Blazor`は`Blazor Server`
    - `_Host.cshtml`に`@attribute [AllowAnonymous]`をつけて,
      各ページに`@attribute [AllowAnonymous]`,
      `@attribute [Authorize]`をつけると適切に認証振り分けされる
- JWTあり: 詳しくは個別の`README`参照
- `TODO`: `Blazor Server`での`JWT`認証
- オリジナルは[Pro ASP.NET Core 6 ](https://github.com/Apress/pro-asp.net-core-6/tree/main/39%20-%20ASP.NET%20Core%20Identity%20-%202/End%20of%20Chapter)
### MainSample
- いろいろな要素を入れたメインサンプル(にしたい)
- `TODO`: シード: ユーザーまで含めて
    - [Proper way to seed test data in EF Core 6.0.2](https://github.com/dotnet/efcore/issues/27599)
    - [One To Many relationship data seeding in .NET Core](https://stackoverflow.com/questions/58599942/one-to-many-relationship-data-seeding-in-net-core)
    - [The Fluent API HasData Method](https://www.learnentityframeworkcore.com/configuration/fluent-api/hasdata-method)
    - [Applying Seed Data To The Database](https://www.learnentityframeworkcore.com/migrations/seeding)
    - [Seeding one-to-many relationships](https://github.com/dotnet/efcore/issues/27208)
- ``TODO`: グローバルなエラーハンドリング
    - [Global Error Handling in ASP.NET Core Web API](https://code-maze.com/global-error-handling-aspnetcore/)
    - [Centralize your .NET Core exception handling with filters](https://medium.com/vx-company/centralize-your-net-exception-handling-with-filters-a1e0fccf17b8)
    - [Best Practices for Exception Handling in .NET Core](https://www.thecodebuzz.com/best-practices-for-handling-exception-in-net-core-2-1/)
- `TODO`: グローバルなロギング
- メモ: BlazorGRPC書く
### MvcWithApi
- `DB`: `SQLite`
- MVCの公式チュートリアルが大元
    - [公式](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-6.0&tabs=visual-studio)
    - [ASP.NET Core 6 MVCのチュートリアルを優しく解説](https://masa7blog.com/asp-net-core-6-mvc-tutorial/)
- MVCとAPIを同居させたサンプルプロジェクト
- APIは下記`SimpleWebApi`と同じく[チュートリアル](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio)から
### RazorPages
- `DB`: `SQLite`
- [チュートリアル: ASP.NET Core の Razor Pages の概要](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-6.0&tabs=visual-studio-code)
- `MvcWithApi`の`MVC`チュートリアル部分を`Razor Pages`に変えただけ
- ASP.NET Coreからはふつうの`MVC`よりこちらが推奨とのこと
- [記事](https://qiita.com/gushwell/items/59ca85fdfee0affa4821)を参考に`Pages/Index.cshtml`に`QRCoder`によるQRコード生成を追加した
- `TODO`: テスト
### SerilogDemo
- [公式](https://serilog.net/)
- その他参考
    - [How to Use Serilog in ASP.NET Core Web API](https://www.claudiobernasconi.ch/2022/01/28/how-to-use-serilog-in-asp-net-core-web-api/)
    - [Add logging to ASP.NET Core using Serilog - .NET6](https://blog.christian-schou.dk/use-serilog-with-asp-net-core-net6/)
- ファイルへのログ書き出し
- `API`と`Razor Pages`それぞれに仕込む
- コンソール・ローカルのファイル・AWSなどいろいろなところに同時に書き込める
- cf. `AWS`では(適当な仮定のもと)標準出力にログを書き出すと`CloudWatch`で拾ってくれる.
### SimpleTest
- `DB`: なし
- モデルクラスが一つだけあるテストを書いたプロジェクト
- データソースが`DI`で抽象化されたコントローラーをテストしている
### SimpleWebApi
- `DB`: インメモリ
- シンプルなAPIのサンプル
### SportsSln: [Pro ASP.NET Core 6, 2022](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205)のサンプルコード
- `SportsSlnOrig`を修正していろいろテスト・確認
### SportsSlnOrig: [Pro ASP.NET Core 6, 2022](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205)のサンプルコード
- `DB`: `docker`で`PostgreSQL`利用
- ある程度の規模があるサンプルプロジェクト
- テストも書いてある
- [ページネーション](https://www.mikesdotnetting.com/article/328/simple-paging-in-asp-net-core-razor-pages)
    - `Pages/List.cshtml`, `Shared/Components/Pager.cshtml`

## TODO
- [Razor Pagesのテスト](https://learn.microsoft.com/ja-jp/aspnet/core/test/razor-pages-tests?view=aspnetcore-7.0)
- EF Coreで中間テーブルを作る
- Terraform・AWS連携
- [メール送信](https://learn.microsoft.com/ja-jp/aspnet/web-pages/overview/getting-started/11-adding-email-to-your-web-site)
- ワンタイムURL(ワンタイムトークン)
- ファイルアップロード
- `Blazor`の実装一般
- [`Blazor WebAssembly`での認証](https://blazor-master.com/identity-server-auth/)
- [Blazor公式](https://dotnet.microsoft.com/ja-jp/apps/aspnet/web-apps/blazor)
- `Blazor WebAssembly`の認証
- テスト
    - `Blazor Server`のテスト
    - [コンテナ活用](https://github.com/testcontainers/testcontainers-dotnet)
    - [Running Tests with Docker](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-tests-in-sdk-container.md)
- [Giraffe](https://github.com/giraffe-fsharp/Giraffe)/[Saturn](https://saturnframework.org/)
    - [Giraffe samples](https://github.com/giraffe-fsharp/samples)
