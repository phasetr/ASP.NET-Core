#r "nuget: SQLProvider"

open FSharp.Data.Sql
let [<Literal>] sqlServConnStr = "Server=localhost,1433;Database=GrateSample;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True"
type sqlServer = SqlDataProvider<
  ConnectionString = sqlServConnStr,
  IndividualsAmount = 1000,
  DatabaseVendor = Common.DatabaseProviderTypes.MSSQLSERVER,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION>

let sqlServCtx = sqlServer.GetDataContext()

// Usersテーブルからデータを取得するクエリ
let users =
    query {
        for user in sqlServCtx.Dbo.Users do
        select (user.Id, user.Name)
    } |> Seq.toList

// 結果を表示
users |> List.iter (fun (id, name) -> printfn "Id: %d, Name: %s" id name)
