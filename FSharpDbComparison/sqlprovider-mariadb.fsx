#r "nuget: SQLProvider"
#r "nuget: MySqlConnector"
open FSharp.Data.Sql

// dllのコピー
let home = System.Environment.GetEnvironmentVariable("HOME")
System.IO.File.Copy($"{home}/.nuget/packages/mysqlconnector/2.3.7/lib/net8.0/MySqlConnector.dll", "MySqlConnector.dll", true)

let [<Literal>] connStr = "Server=localhost;Port=3306;Database=mydb;User Id=user;Password=pass;"
let [<Literal>] resPath = __SOURCE_DIRECTORY__ + "/"
type sql = SqlDataProvider<
  DatabaseVendor = Common.DatabaseProviderTypes.MYSQL,
  ConnectionString = connStr,
  ResolutionPath = resPath,
  IndividualsAmount = 1000,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION,
  Owner="mydb">

let ctx = sql.GetDataContext()

let users =
    query {
        for user in ctx.Mydb.Users do
        select (user.Id, user.Name)
    } |> Seq.toList

query {
    for user in ctx.Mydb.Users do
    select (user.Id, user.Name)
}
|> Seq.toList
|> printfn "%A"
