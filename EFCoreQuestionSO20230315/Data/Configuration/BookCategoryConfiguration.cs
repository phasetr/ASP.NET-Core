using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.HasKey(bc => new {bc.BookId, bc.CategoryId});
        builder.HasData(new {BookId = 1, CategoryId = 1});
    }
}
