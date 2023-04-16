using BlazorWasmHosted.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorWasmHosted.Server.Data.Configuration;

public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    private static readonly Shop Shop1 = new() {Id = 1, Name = "My Shop1"};
    private static readonly Shop Shop2 = new() {Id = 2, Name = "My Shop2"};

    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasData(Shop1, Shop2);
    }
}
