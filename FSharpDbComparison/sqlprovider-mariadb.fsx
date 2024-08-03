#r "nuget: SQLProvider"
#r "nuget: MySqlConnector"
open FSharp.Data.Sql

// copy dll
let home = System.Environment.GetEnvironmentVariable("HOME")
System.IO.File.Copy($"{home}/.nuget/packages/mysqlconnector/2.3.7/lib/net8.0/MySqlConnector.dll", "MySqlConnector.dll", true)
let [<Literal>] mariaConnStr = "Server=localhost;Port=3306;Database=mydb;User Id=user;Password=pass;"
let [<Literal>] mariaResPath = __SOURCE_DIRECTORY__ + "/"
type maria = SqlDataProvider<
  DatabaseVendor = Common.DatabaseProviderTypes.MYSQL,
  ConnectionString = mariaConnStr,
  ResolutionPath = mariaResPath,
  IndividualsAmount = 1000,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION,
  Owner="mydb">

let mariaCtx = maria.GetDataContext()

query {
    for user in mariaCtx.Mydb.Users do
    select (user.Id, user.Name)
}
|> Seq.toList
|> printfn "%A"
