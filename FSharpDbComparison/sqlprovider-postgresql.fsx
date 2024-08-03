#r "nuget: SQLProvider"
#r "nuget: Npgsql"
open FSharp.Data.Sql

// copy dll
let home = System.Environment.GetEnvironmentVariable("HOME")
System.IO.File.Copy($"{home}/.nuget/packages/npgsql/8.0.3/lib/net8.0/Npgsql.dll", "Npgsql.dll", true)
let [<Literal>] pgsqlConnStr = "Host=localhost;Port=5432;Database=mydb;Username=user;Password=pass"
let [<Literal>] pgsqlResPath = __SOURCE_DIRECTORY__ + "/"
type pgsql = SqlDataProvider<
  DatabaseVendor = Common.DatabaseProviderTypes.POSTGRESQL,
  ConnectionString = pgsqlConnStr,
  ResolutionPath = pgsqlResPath,
  IndividualsAmount = 1000,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION,
  Owner="public">

let pgsqlCtx = pgsql.GetDataContext()

query {
    for user in pgsqlCtx.Public.Users do
    select (user.Id, user.Name)
}
|> Seq.toList
|> printfn "%A"
