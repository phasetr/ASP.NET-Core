using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasData(new Category {Id = 1, Name = "category1"}, new Category {Id = 2, Name = "category2"});
    }
}
