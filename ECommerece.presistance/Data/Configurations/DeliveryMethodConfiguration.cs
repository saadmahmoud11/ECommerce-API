using ECommerece.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerece.presistance.Data.Configurations;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(d => d.Price)
            .HasPrecision(8, 2);
        builder.Property(d => d.ShortName)
            .HasMaxLength(50);
        builder.Property(d => d.DeliveryTime)
            .HasMaxLength(50);
        builder.Property(d => d.Description)
            .HasMaxLength(100);
    }
}
