#r "nuget: FSharp.Data"
#r "nuget: SQLProvider"
#r "nuget: Microsoft.Data.Sqlite"

open FSharp.Data.Sql

// dllのコピー
let home = System.Environment.GetEnvironmentVariable("HOME")
System.IO.File.Copy($"{home}/.nuget/packages/microsoft.data.sqlite.core/8.0.7/lib/net8.0/Microsoft.Data.Sqlite.dll", "Microsoft.Data.Sqlite.dll", true)

let [<Literal>] sqliteConnStr = "Data Source=grate-sqlite.db"
type sqlite = SqlDataProvider<
  DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
  SQLiteLibrary = Common.SQLiteLibrary.MicrosoftDataSqlite,
  ConnectionString = sqliteConnStr,
  IndividualsAmount = 1000,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION>

// データベースの操作例
let sqliteCtx = sqlite.GetDataContext()

let users =
    query {
        for user in sqliteCtx.Main.Users do
        select (user.Id, user.Name)
    } |> Seq.toList
users |> printfn "%A"
