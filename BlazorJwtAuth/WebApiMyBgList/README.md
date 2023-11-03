# README

- [GitHub, Chapter_12](https://github.com/Darkseal/ASP.NET-Core-Web-API)

## 初期化

- 次のコマンドを実行

```shell
dotnet tool restore
efm add Initialize
efd update
```

- アプリケーションを起動して`Swagger`で`SeedController`を実行する（`Auth`の分は初期値として設定済み）

## TODO

- `SQL Server`を`SQLite`に置き換える
- `Minimal API`のファイル移動

## 本の記録

- [ミドルウェアの指定順に関する公式ドキュメント](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order)

### Chapter 3

- `DTO`統一インターフェース
- `SemVer`ベースの`API`バージョン管理システムを`ASP.NET Core`に実装する最も効果的な方法: 次の`NuGet`パッケージをインストールする
  - `Microsoft.AspNetCore.Mvc.Versioning`
  - `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer`
  - `[ApiVersion]`属性

### Chapter 4

- データベース・`EF Core`
- [EF Coreのデータ注釈属性](https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt)

### Chapter 6

- バリデーション
- カスタム検証属性, `ValidationAttribute`
- `IValidatableObject`インターフェースはクラスを検証するための代替方法を提供
- `/Attributes`ディレクトリ
- `[FromQuery]`: クエリ文字列から値を取得
- `[FromRoute]`: ルートデータから値を取得
- `[FromForm]`: 投稿されたフォームフィールドから値を取得
- `[FromBody]`: リクエスト本文から値を取得
- `[FromHeader]`: HTTP ヘッダーから値を取得
- `[FromServices]`: 登録されたサービスのインスタンスから値を取得
- `[FromUri]`: 外部 URI から値を取得

### Chapter 7

- ログ、`Serilog`

### Chapter 9

- 認証・認可
- ここでは`JWT`のベアラートークン認証
- `Min18`の認可設定を実装

### Chapter 10

- `GraphQL`は[HotChocolate](https://github.com/ChilliCream/hotchocolate) 
