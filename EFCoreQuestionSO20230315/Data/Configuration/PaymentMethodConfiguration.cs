using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.HasKey(m => new {m.ShopId, m.Name});
        builder.HasData(
            new PaymentMethod
            {
                Name = "Cash",
                ShopId = ShopConfiguration.Shops[0].Id
            },
            new PaymentMethod
            {
                Name = "Credit Card",
                ShopId = ShopConfiguration.Shops[0].Id
            }
        );
    }
}
