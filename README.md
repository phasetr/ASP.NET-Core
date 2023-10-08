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
- `compose.yml`: データベースだけのファイル
- `compose.with-dotnet.yml`: データベースに加えて`.NET`の開発用コンテナも含む

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

### Blazor UI Framework

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

## 各ディレクトリの説明

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

### SportsSln: [Pro ASP.NET Core 6, 2022](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205)のサンプルコード

- `SportsSlnOrig`を修正していろいろテスト・確認

### SportsSlnOrig: [Pro ASP.NET Core 6, 2022](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205)のサンプルコード

- `DB`: `docker`で`PostgreSQL`利用
- ある程度の規模があるサンプルプロジェクト
- テストも書いてある
- [ページネーション](https://www.mikesdotnetting.com/article/328/simple-paging-in-asp-net-core-razor-pages)
  - `Pages/List.cshtml`, `Shared/Components/Pager.cshtml`
