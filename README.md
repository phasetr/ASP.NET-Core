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
