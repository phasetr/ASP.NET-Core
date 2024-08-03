#r "nuget: SQLProvider";
#r "nuget: Microsoft.Data.Sqlite"
open FSharp.Data.Sql

// copy dll
let home = System.Environment.GetEnvironmentVariable("HOME")
System.IO.File.Copy($"{home}/.nuget/packages/microsoft.data.sqlite.core/8.0.7/lib/net8.0/Microsoft.Data.Sqlite.dll", "Microsoft.Data.Sqlite.dll", true)
let [<Literal>] sqliteConnStr = "Data Source=grate-sqlite.db"
let [<Literal>] sqliteResPath = __SOURCE_DIRECTORY__ + "/"
type sqlite = SqlDataProvider<
  DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
  SQLiteLibrary = Common.SQLiteLibrary.MicrosoftDataSqlite,
  ConnectionString = sqliteConnStr,
  ResolutionPath = sqliteResPath,
  IndividualsAmount = 1000,
  UseOptionTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION>

let sqliteCtx = sqlite.GetDataContext()

query {
    for user in sqliteCtx.Main.Users do
    select (user.Id, user.Name)
}
|> Seq.toList
|> printfn "%A"

// https://fsprojects.github.io/SQLProvider//core/querying.html
// SQL句の確認
FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent
|> Event.add (printfn "Executing SQL: %O")

// 同期版
query {
    for user in sqliteCtx.Main.Users do
    select user
}
|> Seq.toArray
|> Array.map(fun i -> i.ColumnValues |> Map.ofSeq)

// 非同期版
task {
    let! res =
       query {
           for user in sqliteCtx.Main.Users do
           select user
       } |> Seq.executeQueryAsync
    return res
}
|> Async.AwaitTask
|> Async.RunSynchronously

// https://fsprojects.github.io/SQLProvider//core/crud.html
// CRUD
sqliteCtx.Main.Courses |> Seq.head

let course = sqliteCtx.Main.Courses.Create()
course.Title <- "EF Core Course3"
sqliteCtx.SubmitUpdates()
query {
    for c in sqliteCtx.Main.Courses do
    select (c.Id, c.Title)
}
|> Seq.toList
|> printfn "%A"
