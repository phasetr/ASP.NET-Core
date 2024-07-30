namespace EFCoreCSharp;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<UserCourse> UserCourses { get; set; }
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public ICollection<UserCourse> UserCourses { get; set; }
    public ICollection<CourseChapter> CourseChapters { get; set; }
}

public class Chapter
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public ICollection<CourseChapter> CourseChapters { get; set; }
}

public class UserCourse
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
}

public class CourseChapter
{
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; }
}