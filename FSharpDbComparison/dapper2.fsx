#r "nuget: Dapper.FSharp"
#r "nuget: Microsoft.Data.Sqlite"

open Microsoft.Data.Sqlite
open Dapper
open Dapper.FSharp.SQLite

OptionTypes.register()

type User = {
  Id: int
  Name: string
}
type Course = {
  Id: int
  Title: string
}
type Chapter = {
  Id: int
  Title: string
}
type UserCourse = {
  UserId: int
  CourseId: int
}
type CourseChapter = {
  CourseId: int
  ChapterId: int
}
// テーブル名を複数にするには`table'`を使用
let userTable = table'<User> "Users"
let courseTable = table'<Course> "Courses"
let chapterTable = table'<Chapter> "Chapters"
let userCourseTable = table'<UserCourse> "UserCourses"
let courseChapterTable = table'<CourseChapter> "CourseChapters"

// SQLite接続文字列
let connStr = "Data Source=dapper2.db"

// テーブル作成スクリプト
let initializeDatabase() =
    use connection = new SqliteConnection(connStr)
    connection.Open()
    let sql = """
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY,
    Name TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Courses (
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Chapters (
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS UserCourses (
    UserId INTEGER NOT NULL,
    CourseId INTEGER NOT NULL,
    PRIMARY KEY (UserId, CourseId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);
CREATE TABLE IF NOT EXISTS CourseChapters (
    CourseId INTEGER NOT NULL,
    ChapterId INTEGER NOT NULL,
    PRIMARY KEY (CourseId, ChapterId),
    FOREIGN KEY (CourseId) REFERENCES Courses(Id),
    FOREIGN KEY (ChapterId) REFERENCES Chapters(Id)
);
    """
    connection.Execute(sql) |> ignore

let insertData() =
    use conn = new SqliteConnection(connStr)
    conn.Open()

    let users = [
        {Id = 1; Name = "John Doe"}
        {Id = 2; Name = "Jane Doe"}
    ]
    let userSql = "INSERT INTO Users (Id, Name) VALUES (@Id, @Name)"
    users |> List.iter (fun user -> conn.Execute(userSql, user) |> ignore)

    let course1 = {Id = 1; Title = "EF Core Course"}
    let course2 = {Id = 2; Title = "F# Course"}
    let courses = [ course1; course2   ]
    let courseSql = "INSERT INTO Courses (Id, Title) VALUES (@Id, @Title)"
    courses |> List.iter (fun course -> conn.Execute(courseSql, course) |> ignore)

    let chapter1 = {Id = 1; Title = "Introduction"}
    let chapter2 =    {Id = 2; Title = "Advanced Topics"}
    let chapters = [chapter1;chapter2    ]
    let chapterSql = "INSERT INTO Chapters (Id, Title) VALUES (@Id, @Title)"
    chapters |> List.iter (fun chapter -> conn.Execute(chapterSql, chapter) |> ignore)

    let userCourses = [
        {UserId = 1; CourseId = 1}
    ]
    let userCourseSql = "INSERT INTO UserCourses (UserId, CourseId) VALUES (@UserId, @CourseId)"
    userCourses |> List.iter (fun uc -> conn.Execute(userCourseSql, uc) |> ignore)

    let courseChapters = [
        {CourseId = 1; ChapterId = 1}
        {CourseId = 1; ChapterId = 2}
    ]
    let courseChapterSql = "INSERT INTO CourseChapters (CourseId, ChapterId) VALUES (@CourseId, @ChapterId)"
    courseChapters |> List.iter (fun cc -> conn.Execute(courseChapterSql, cc) |> ignore)

initializeDatabase()
insertData()

let conn = new SqliteConnection(connStr)
async {
    let! users =
        conn.QueryAsync<User>("SELECT * FROM Users")
        |> Async.AwaitTask
    return users
}
|> Async.RunSynchronously
|> printfn "%A"

select {
  for u in userTable do
  innerJoin uc in userCourseTable on (u.Id = uc.UserId)
  orderBy u.Id
}
|> conn.SelectAsyncOption<User, UserCourse>
|> Async.AwaitTask
|> Async.RunSynchronously
|> printfn "%A"
