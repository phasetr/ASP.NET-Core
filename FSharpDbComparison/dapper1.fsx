#r "nuget: Dapper.FSharp"
#r "nuget: Microsoft.Data.Sqlite"

open Microsoft.Data.Sqlite
open Dapper
open Dapper.FSharp.SQLite

OptionTypes.register()

// 定義
type User = {
    UserId: int
    Name: string
    Email: string
}
type Course = {
    CourseId: int
    CourseName: string
}
type UserCourse = {
    UserId: int
    CourseId: int
}
let userTable = table<User>
let courseTable = table<Course>
let userCourseTable = table<UserCourse>

// SQLiteの接続文字列
let connString = "Data Source=dapper1.db"

// データベース初期化
let initializeDatabase () =
    use conn = new SqliteConnection(connString)
    conn.Open()
    let sql = """
        CREATE TABLE IF NOT EXISTS User (
            UserId INTEGER PRIMARY KEY,
            Name TEXT NOT NULL,
            Email TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS Course (
            CourseId INTEGER PRIMARY KEY,
            CourseName TEXT NOT NULL
        );
        CREATE TABLE IF NOT EXISTS UserCourse (
            UserId INTEGER NOT NULL,
            CourseId INTEGER NOT NULL,
            PRIMARY KEY (UserId, CourseId),
            FOREIGN KEY (UserId) REFERENCES User(UserId),
            FOREIGN KEY (CourseId) REFERENCES Course(CourseId)
        );
    """
    conn.Execute(sql) |> ignore

// 初期データ挿入
let insertData () =
    use conn = new SqliteConnection(connString)
    conn.Open()

    // ユーザーの追加
    let users = [
        { UserId = 1; Name = "Alice"; Email = "alice@example.com" }
        { UserId = 2; Name = "Bob"; Email = "bob@example.com" }
    ]
    let userSql = "INSERT INTO User (UserId, Name, Email) VALUES (@UserId, @Name, @Email)"
    users |> List.iter (fun user -> conn.Execute(userSql, user) |> ignore)

    // コースの追加
    let courses = [
        { CourseId = 1; CourseName = "Math" }
        { CourseId = 2; CourseName = "Science" }
    ]
    let courseSql = "INSERT INTO Course (CourseId, CourseName) VALUES (@CourseId, @CourseName)"
    courses |> List.iter (fun course -> conn.Execute(courseSql, course) |> ignore)

    // ユーザーコースの追加
    let userCourses = [
        { UserId = 1; CourseId = 1 }
        { UserId = 1; CourseId = 2 }
        { UserId = 2; CourseId = 1 }
    ]
    let userCourseSql = "INSERT INTO UserCourse (UserId, CourseId) VALUES (@UserId, @CourseId)"
    userCourses |> List.iter (fun uc -> conn.Execute(userCourseSql, uc) |> ignore)

// データベース初期化とデータ挿入の実行
initializeDatabase()
insertData()

printfn "Database initialized and data inserted."

// 何度も使うためユーザーの取得を関数化
let getAllUser =
  use conn = new SqliteConnection(connString)
  async {
    let! users = conn.QueryAsync<User>("SELECT * FROM User") |> Async.AwaitTask
    return users
  }
  |> Async.RunSynchronously

// INSERT & DELETE
let conn = new SqliteConnection(connString)
let newUser = { UserId = 10; Name = "Roman"; Email = "test@gmail.com" }
insert {
  into userTable
  value newUser
} |> conn.InsertAsync
delete {
  for u in userTable do
  where (u.UserId = 10)
} |> conn.DeleteAsync
getAllUser |> Seq.iter (fun user -> printfn $"%A{user}")

// SELECT
async {
    let! users = conn.QueryAsync<User>("SELECT * FROM User") |> Async.AwaitTask
    return users
}
|> Async.RunSynchronously
|> Seq.iter (fun user -> printfn $"%A{user}")

select {
  for _ in userTable do selectAll
}
|> conn.SelectAsync<User>
|> Async.AwaitTask
|> Async.RunSynchronously
|> Seq.iter (fun user -> printfn $"%A{user}")

// JOIN
select {
  for u in userTable do
  innerJoin uc in userCourseTable on (u.UserId = uc.UserId)
  orderBy u.UserId
}
|> conn.SelectAsyncOption<User, UserCourse>
|> Async.AwaitTask
|> Async.RunSynchronously

select {
  for u in userTable do
  innerJoin uc in userCourseTable on (u.UserId = uc.UserId)
  leftJoin c in courseTable on (uc.CourseId = c.CourseId)
  orderBy u.UserId
}
|> conn.SelectAsyncOption<User, UserCourse, Course>
|> Async.AwaitTask
|> Async.RunSynchronously
