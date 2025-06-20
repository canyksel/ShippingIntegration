using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class ShippingCompanyEntityTypeConfiguration : IEntityTypeConfiguration<ShippingCompany>
{
    public void Configure(EntityTypeBuilder<ShippingCompany> builder)
    {
        builder.ToTable("ShippingCompanies");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Code).HasMaxLength(10).IsRequired();

        builder.HasOne<CompanyAddress>()
              .WithMany()
              .HasForeignKey("CompanyAddressId")
              .OnDelete(DeleteBehavior.Restrict);
    }
}
