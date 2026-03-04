using ECommerece.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerece.presistance.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Subtotal)
            .HasPrecision(8,2);
        builder.OwnsOne(o => o.OrderAddress, oEntity =>
        {
            oEntity.Property(a => a.FirstName).HasMaxLength(50);
            oEntity.Property(a => a.LastName).HasMaxLength(50);
            oEntity.Property(a => a.Street).HasMaxLength(50);
            oEntity.Property(a => a.City).HasMaxLength(50);
            oEntity.Property(a => a.Country).HasMaxLength(50);
        });
    }
}
