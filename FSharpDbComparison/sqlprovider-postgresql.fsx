#r "nuget: SQLProvider"
#r "nuget: Npgsql"
open FSharp.Data.Sql

// dllのコピー
let home = System.Environment.GetEnvironmentVariable("HOME")
System.IO.File.Copy($"{home}/.nuget/packages/npgsql/8.0.3/lib/net8.0/Npgsql.dll", "Npgsql.dll", true)

let [<Literal>] pgsqlConnStr = "Host=localhost;Port=5432;Database=mydb;Username=user;Password=pass"
let [<Literal>] pgsqlResPath = __SOURCE_DIRECTORY__ + "/"
let [<Literal>] pgsqlOwner = "public"
type pgsql = SqlDataProvider<
  DatabaseVendor = Common.DatabaseProviderTypes.POSTGRESQL,
  ConnectionString = pgsqlConnStr,
  ResolutionPath = pgsqlResPath,
  IndividualsAmount = 1000,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION,
  Owner=pgsqlOwner>

let ctx = pgsql.GetDataContext()

let users =
    query {
        for user in ctx.Public.Users do
        select (user.Id, user.Name)
    } |> Seq.toList
users |> printfn "%A"

query {
    for user in ctx.Public.Users do
    select (user.Id, user.Name)
}
|> Seq.toList
|> printfn "%A"
