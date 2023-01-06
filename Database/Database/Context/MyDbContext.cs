using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartLine> CartLines { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartLine>(entity =>
        {
            entity.ToTable("CartLine");

            entity.HasComment("CartLine");

            entity.HasIndex(e => e.OrderId, "IX_CartLine_OrderId");

            entity.HasIndex(e => e.ProductId, "IX_CartLine_ProductId");

            entity.Property(e => e.CartLineId)
                .ValueGeneratedNever()
                .HasColumnName("CartLineId")
                .HasComment("CartLineId");

            entity.Property(e => e.OrderId)
                .HasColumnName("OrderId")
                .HasComment("OrderId");

            entity.Property(e => e.ProductId)
                .HasColumnName("ProductId")
                .HasComment("ProductId");

            entity.Property(e => e.Quantity).HasComment("Quantity");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasComment("Orders");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("OrderId")
                .HasComment("OrderId");

            entity.Property(e => e.City).HasComment("City");

            entity.Property(e => e.Country).HasComment("Country");

            entity.Property(e => e.GiftWrap).HasComment("GiftWrap");

            entity.Property(e => e.Line1).HasComment("Line1");

            entity.Property(e => e.Line2).HasComment("Line2");

            entity.Property(e => e.Line3).HasComment("Line3");

            entity.Property(e => e.Name).HasComment("Name");

            entity.Property(e => e.Shipped).HasComment("Shipped");

            entity.Property(e => e.State).HasComment("State");

            entity.Property(e => e.Zip).HasComment("Zip");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasComment("OrderDetails");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Id");

            entity.Property(e => e.OrderId)
                .HasColumnName("OrderId")
                .HasComment("OrderId");

            entity.Property(e => e.ProductId)
                .HasColumnName("ProductId")
                .HasComment("ProductId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasComment("Products");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductId")
                .HasComment("ProductId");

            entity.Property(e => e.Category).HasComment("Category");

            entity.Property(e => e.Description).HasComment("Description");

            entity.Property(e => e.Name).HasComment("Name");

            entity.Property(e => e.Price)
                .HasPrecision(8, 2)
                .HasComment("Price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
