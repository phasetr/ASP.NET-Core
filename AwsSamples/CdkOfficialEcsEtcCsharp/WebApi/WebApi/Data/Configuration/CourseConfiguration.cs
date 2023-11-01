using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Data.Configuration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasData(new Course
        {
            Id = 1,
            Name = "Math"
        }, new Course
        {
            Id = 2,
            Name = "Physics"
        }, new Course
        {
            Id = 3,
            Name = "English"
        });
    }
}
