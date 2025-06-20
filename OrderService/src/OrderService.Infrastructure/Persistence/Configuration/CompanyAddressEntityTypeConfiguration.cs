using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class CompanyAddressEntityTypeConfiguration : IEntityTypeConfiguration<CompanyAddress>
{
    public void Configure(EntityTypeBuilder<CompanyAddress> builder)
    {
        builder.ToTable("CompanyAddresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Country).HasMaxLength(100).IsRequired();
        builder.Property(a => a.City).HasMaxLength(100).IsRequired();
        builder.Property(a => a.State).HasMaxLength(100).IsRequired();
        builder.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
        builder.Property(a => a.AddressTitle).HasMaxLength(100).IsRequired();
        builder.Property(a => a.AddressDetail).HasMaxLength(250).IsRequired();
    }
}