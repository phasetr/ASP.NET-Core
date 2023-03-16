using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(new Book {Id = 1, Title = "book1"}, new Book {Id = 2, Title = "book2"});
    }
}
