using ECommerece.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerece.presistance.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.Price)
            .HasPrecision(8, 2);
        builder.OwnsOne(p => p.Product, oEntity =>
        {
            oEntity.Property(p => p.ProductName)
            .HasMaxLength(100);
            oEntity.Property(p => p.PictureUrl)
            .HasMaxLength(200);
        });
    }
}
