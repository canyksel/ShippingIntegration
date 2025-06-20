using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class ShippingCompanyEntityTypeConfiguration : IEntityTypeConfiguration<ShippingCompany>
{
    public void Configure(EntityTypeBuilder<ShippingCompany> builder)
    {
        builder.ToTable("ShippingCompanies");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.Code)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(s => s.ShipmentDate);

        builder.Property(s => s.CompanyAddressId)
               .IsRequired();

        builder.HasOne(s => s.Address)
               .WithMany()
               .HasForeignKey(s => s.CompanyAddressId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
