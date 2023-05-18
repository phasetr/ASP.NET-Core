using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "Product1",
                Price = 10,
                ShopId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Product2",
                Price = 20,
                ShopId = 1,
                IsDeleted = true
            },
            new Product
            {
                Id = 3,
                Name = "Product3",
                Price = 30,
                ShopId = 1
            },
            new Product
            {
                Id = 4,
                Name = "Product4",
                Price = 40,
                ShopId = 1
            },
            new Product
            {
                Id = 5,
                Name = "Product5",
                Price = 50,
                ShopId = 1
            },
            new Product
            {
                Id = 6,
                Name = "Product6",
                Price = 60,
                ShopId = 1
            },
            new Product
            {
                Id = 7,
                Name = "Product7",
                Price = 70,
                ShopId = 1
            },
            new Product
            {
                Id = 8,
                Name = "Product8",
                Price = 80,
                ShopId = 1
            }
        );
    }
}
