#r "nuget: FSharp.Data"
#r "nuget: SQLProvider"
#r "nuget: Microsoft.Data.Sqlite"
#r "nuget: SQLitePCLRaw.bundle_e_sqlite3"
#r "nuget: SQLitePCLRaw.core"

open FSharp.Data.Sql
open System.Reflection

// Microsoft.Data.Sqlite.dllのコピー
let home = System.Environment.GetEnvironmentVariable("HOME")
System.IO.File.Copy($"{home}/.nuget/packages/microsoft.data.sqlite.core/8.0.7/lib/net8.0/Microsoft.Data.Sqlite.dll", "Microsoft.Data.Sqlite.dll", true)

Assembly.Load("SQLitePCLRaw.core, Version=2.1.6.2060, Culture=neutral, PublicKeyToken=1488e028ca7ab535")
let [<Literal>] connStr = "Data Source=efcore-fsharp.tmp.tmp.db"
let [<Literal>] resolutionPath = "."
type sql = SqlDataProvider<
  ConnectionString = connStr,
  ResolutionPath = resolutionPath,
  DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
  IndividualsAmount = 1000,
  CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION>

// データベースの操作例
let ctx = sql.GetDataContext()

let allUsers =
    query {
        for user in ctx.Main.Users do
        select user
    } |> Seq.toList
allUsers |> List.iter (fun user -> printfn $"%A{user}")
