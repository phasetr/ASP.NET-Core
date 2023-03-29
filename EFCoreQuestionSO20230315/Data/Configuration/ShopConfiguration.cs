using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public static readonly Shop[] Shops =
    {
        new()
        {
            Id = 1,
            Name = "Shop1"
        },
        new()
        {
            Id = 2,
            Name = "Shop2"
        }
    };

    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasData(Shops);
    }
}
