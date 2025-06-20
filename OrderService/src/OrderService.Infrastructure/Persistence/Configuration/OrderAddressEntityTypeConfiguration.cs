using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class OrderAddressEntityTypeConfiguration : IEntityTypeConfiguration<OrderAddress>
{
    public void Configure(EntityTypeBuilder<OrderAddress> builder)
    {
        builder.ToTable("OrderAddresses");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Country).HasMaxLength(100).IsRequired();
        builder.Property(p => p.City).HasMaxLength(100).IsRequired();
        builder.Property(p => p.State).HasMaxLength(100).IsRequired();
        builder.Property(p => p.PostalCode).HasMaxLength(20).IsRequired();
        builder.Property(p => p.AddressTitle).HasMaxLength(100).IsRequired();
        builder.Property(p => p.AddressDetail).HasMaxLength(250).IsRequired();
    }
}