#r "nuget: Microsoft.EntityFrameworkCore.Sqlite"
#r "nuget: Microsoft.EntityFrameworkCore.Design"
#r "nuget: System.Linq"
#r "nuget: EntityFrameworkCore.FSharp"

open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open EntityFrameworkCore.FSharp.Extensions

let dbname = "efcore-fsharp.tmp.tmp.db"

[<CLIMutable>]
type User = {
  Id: int
  Name: string
  UserCourses: UserCourse list
}
and [<CLIMutable>]
Course = {
  Id: int
  Title: string
  UserCourses: UserCourse list
  CourseChapters: CourseChapter list
}
and [<CLIMutable>]
Chapter = {
  Id: int
  Title: string
  CourseChapters: CourseChapter list
}
and [<CLIMutable>]
UserCourse = {
  UserId: int
  User: User
  CourseId: int
  Course: Course
}
and [<CLIMutable>]
CourseChapter = {
  CourseId: int
  Course: Course
  ChapterId: int
  Chapter: Chapter
}

type AppDbContext() =
    inherit DbContext()

    [<DefaultValue>]
    val mutable users : DbSet<User>
    member this.Users with get() = this.users and set v = this.users <- v
    [<DefaultValue>]
    val mutable courses : DbSet<Course>
    member this.Courses with get() = this.courses and set v = this.courses <- v
    [<DefaultValue>]
    val mutable chapters : DbSet<Chapter>
    member this.Chapters with get() = this.chapters and set v = this.chapters <- v
    [<DefaultValue>]
    val mutable userCourses : DbSet<UserCourse>
    member this.UserCourses with get() = this.userCourses and set v = this.userCourses <- v
    [<DefaultValue>]
    val mutable courseChapters : DbSet<CourseChapter>
    member this.CourseChapters with get() = this.courseChapters and set v = this.courseChapters <- v

    override _.OnConfiguring(options: DbContextOptionsBuilder) : unit =
        options.UseSqlite($"Data Source={dbname}").UseFSharpTypes()
        |> ignore

    override _.OnModelCreating(modelBuilder: ModelBuilder) =
        modelBuilder.RegisterOptionTypes()

        modelBuilder.Entity<UserCourse>(fun uc ->
            uc.HasKey(fun uc -> (uc.UserId, uc.CourseId) :> obj) |> ignore
            uc.HasOne(fun uc -> uc.User)
              .WithMany(fun u -> u.UserCourses :> IEnumerable<UserCourse>)
              .HasForeignKey("UserId") |> ignore
            uc.HasOne(fun uc -> uc.Course)
              .WithMany(fun c -> c.UserCourses :> IEnumerable<UserCourse>)
              .HasForeignKey("CourseId") |> ignore
        ) |> ignore

        modelBuilder.Entity<CourseChapter>(fun cc ->
            cc.HasKey(fun cc -> (cc.CourseId, cc.ChapterId) :> obj) |> ignore
            cc.HasOne(fun cc -> cc.Course)
              .WithMany(fun c -> c.CourseChapters :> IEnumerable<CourseChapter>)
              .HasForeignKey("CourseId") |> ignore
            cc.HasOne(fun cc -> cc.Chapter)
              .WithMany(fun ch -> ch.CourseChapters :> IEnumerable<CourseChapter>)
              .HasForeignKey("ChapterId") |> ignore
        ) |> ignore

let initializeDatabase() =
    use context = new AppDbContext()
    context.Database.EnsureDeleted() |> ignore
    context.Database.EnsureCreated() |> ignore

    // データの追加
    let user = { Id = 1; Name = "John Doe"; UserCourses = [] }
    let course = { Id = 1; Title = "EF Core Course"; UserCourses = []; CourseChapters = [] }
    let chapter1 = { Id = 1; Title = "Introduction"; CourseChapters = [] }
    let chapter2 = { Id = 2; Title = "Advanced Topics"; CourseChapters = [] }

    context.Users.Add(user) |> ignore
    context.Courses.Add(course) |> ignore
    context.Chapters.AddRange([| chapter1; chapter2 |])

    context.SaveChanges() |> ignore

    // 中間テーブルのデータ追加
    let userCourse = { UserId = user.Id; User = user; CourseId = course.Id; Course = course }
    let courseChapter1 = { CourseId = course.Id; Course = course; ChapterId = chapter1.Id; Chapter = chapter1 }
    let courseChapter2 = { CourseId = course.Id; Course = course; ChapterId = chapter2.Id; Chapter = chapter2 }

    context.UserCourses.Add(userCourse) |> ignore
    context.CourseChapters.AddRange([| courseChapter1; courseChapter2 |])

    context.SaveChanges() |> ignore

let displayData() =
    use context = new AppDbContext()
    let users =
      context.Users
        .Include(fun u -> u.UserCourses)
        .ThenInclude(fun uc -> uc.Head)
    let courses =
      context.Courses
        .Include(fun c -> c.CourseChapters)
        .ThenInclude(fun cc -> cc.Head)

    printfn "Users:"
    for u in users do
        printfn $"- %s{u.Name}"
        for uc in u.UserCourses do
            printfn $"  - Enrolled in: %s{uc.Course.Title}"

    printfn "Courses:"
    for c in courses do
        printfn $"- %s{c.Title}"
        for cc in c.CourseChapters do
            printfn $"  - Chapter: %s{cc.Chapter.Title}"

// データベースの初期化とデータの表示
initializeDatabase()
displayData()
