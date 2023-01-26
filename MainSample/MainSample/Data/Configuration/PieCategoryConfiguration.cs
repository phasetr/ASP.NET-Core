using MainSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainSample.Data.Configuration;

public class PieCategoryConfiguration : IEntityTypeConfiguration<PieCategory>
{
    public void Configure(EntityTypeBuilder<PieCategory> builder)
    {
        builder.HasData(new PieCategory {Id = 1, Name = "Fruit pies"});
    }
}