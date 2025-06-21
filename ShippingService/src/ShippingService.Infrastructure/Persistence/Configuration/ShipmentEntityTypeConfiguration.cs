using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingService.Domain.Entities;

namespace ShippingService.Infrastructure.Persistence.Configuration;

public class ShipmentEntityTypeConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.OrderId).IsRequired();
        builder.Property(s => s.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ShippingCompanyId).IsRequired();
        builder.Property(s => s.Status).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.LastUpdatedAt).IsRequired(false);
    }
}