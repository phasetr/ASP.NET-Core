using EFCoreCSharp;
using Microsoft.EntityFrameworkCore;

using var context = new AppDbContext();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

Console.WriteLine(File.Exists("efcore-csharp.db")
    ? "Database file 'efcore-csharp.tmp.db' has been created successfully."
    : "Failed to create the database file 'efcore-csharp.db'.");

// データの追加
var user = new User { Name = "John Doe" };
var course = new Course { Title = "EF Core Course" };
var chapter1 = new Chapter { Title = "Introduction" };
var chapter2 = new Chapter { Title = "Advanced Topics" };

context.Users.Add(user);
context.Courses.Add(course);
context.Chapters.AddRange(chapter1, chapter2);

context.SaveChanges();

// 中間テーブルのデータ追加
var userCourse = new UserCourse { User = user, Course = course };
var courseChapter1 = new CourseChapter { Course = course, Chapter = chapter1 };
var courseChapter2 = new CourseChapter { Course = course, Chapter = chapter2 };

context.UserCourses.Add(userCourse);
context.CourseChapters.AddRange(courseChapter1, courseChapter2);

context.SaveChanges();

// 確認
var users = context.Users.Include(u => u.UserCourses).ThenInclude(uc => uc.Course).ToList();
var courses = context.Courses.Include(c => c.CourseChapters).ThenInclude(cc => cc.Chapter).ToList();

Console.WriteLine("Users:");
foreach (var u in users)
{
    Console.WriteLine($"- {u.Name}");
    foreach (var uc in u.UserCourses) Console.WriteLine($"  - Enrolled in: {uc.Course.Title}");
}

Console.WriteLine("Courses:");
foreach (var c in courses)
{
    Console.WriteLine($"- {c.Title}");
    foreach (var cc in c.CourseChapters) Console.WriteLine($"  - Chapter: {cc.Chapter.Title}");
}
