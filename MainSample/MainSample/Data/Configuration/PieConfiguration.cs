using MainSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainSample.Data.Configuration;

public class PieConfiguration : IEntityTypeConfiguration<Pie>
{
    public void Configure(EntityTypeBuilder<Pie> builder)
    {
        builder.HasData(new Pie {Id = 1, PieCategoryId = 1, Name = "Apple Pie"});
    }
}