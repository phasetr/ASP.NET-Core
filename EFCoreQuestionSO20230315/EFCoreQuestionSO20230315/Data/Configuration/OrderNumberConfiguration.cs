using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class OrderNumberConfiguration : IEntityTypeConfiguration<OrderNumber>
{
    public void Configure(EntityTypeBuilder<OrderNumber> builder)
    {
        var shops = ShopConfiguration.Shops;
        builder.HasData(
            new OrderNumber
            {
                ShopId = shops[0].Id,
                Number = 1
            },
            new OrderNumber
            {
                ShopId = shops[1].Id,
                Number = 1
            }
        );
    }
}
