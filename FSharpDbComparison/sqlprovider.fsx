#r "nuget: FSharp.Data"
#r "nuget: SQLProvider"

open FSharp.Data.Sql
let [<Literal>] connStr = "Data Source=efcore-fsharp.tmp.db"
let [<Literal>] resolutionPath = "~/.nuget/packages/microsoft.data.sqlite.core/8.0.7/lib/net8.0"

type sql = SqlDataProvider<
                ConnectionString = connStr,
                DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
                ResolutionPath = resolutionPath,
                IndividualsAmount = 1000,
                CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL,
                UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION>

// データベースの操作例
let ctx = sql.GetDataContext()

let allUsers = query {
    for user in ctx.Main.Users do
    select user
} |> Seq.toList

allUsers |> List.iter (fun user -> printfn "%A" user)
