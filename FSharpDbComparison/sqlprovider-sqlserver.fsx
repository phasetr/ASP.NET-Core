#r "nuget: SQLProvider"
open FSharp.Data.Sql
let [<Literal>] sqlServConnStr = "Server=localhost,1433;Database=GrateSample;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True"
type sqlServer = SqlDataProvider<
  ConnectionString = sqlServConnStr,
  IndividualsAmount = 1000,
  DatabaseVendor = Common.DatabaseProviderTypes.MSSQLSERVER,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION>

let sqlServCtx = sqlServer.GetDataContext()

query {
    for user in sqlServCtx.Dbo.Users do
    select (user.Id, user.Name)
}
|> Seq.toList
|> printfn "%A"
