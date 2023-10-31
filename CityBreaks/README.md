# README

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
