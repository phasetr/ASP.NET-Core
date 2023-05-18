using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ApplicationUserShopConfiguration : IEntityTypeConfiguration<ApplicationUserShop>
{
    public void Configure(EntityTypeBuilder<ApplicationUserShop> builder)
    {
        builder.HasKey(m => new {m.ApplicationUserId, m.ShopId});
        builder.HasData(new ApplicationUserShop
        {
            ApplicationUserId = ApplicationUserConfiguration.Staff1Guid.ToString(),
            ShopId = ShopConfiguration.Shops[0].Id
        }, new ApplicationUserShop
        {
            ApplicationUserId = ApplicationUserConfiguration.Staff1Guid.ToString(),
            ShopId = ShopConfiguration.Shops[1].Id
        });
    }
}
