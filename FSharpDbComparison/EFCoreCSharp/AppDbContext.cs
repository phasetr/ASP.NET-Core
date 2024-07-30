using Microsoft.EntityFrameworkCore;

namespace EFCoreCSharp;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }
    public DbSet<CourseChapter> CourseChapters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=efcore-csharp.tmp.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCourse>()
            .HasKey(uc => new { uc.UserId, uc.CourseId });

        modelBuilder.Entity<UserCourse>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCourses)
            .HasForeignKey(uc => uc.UserId);

        modelBuilder.Entity<UserCourse>()
            .HasOne(uc => uc.Course)
            .WithMany(c => c.UserCourses)
            .HasForeignKey(uc => uc.CourseId);

        modelBuilder.Entity<CourseChapter>()
            .HasKey(cc => new { cc.CourseId, cc.ChapterId });

        modelBuilder.Entity<CourseChapter>()
            .HasOne(cc => cc.Course)
            .WithMany(c => c.CourseChapters)
            .HasForeignKey(cc => cc.CourseId);

        modelBuilder.Entity<CourseChapter>()
            .HasOne(cc => cc.Chapter)
            .WithMany(ch => ch.CourseChapters)
            .HasForeignKey(cc => cc.ChapterId);
    }
}