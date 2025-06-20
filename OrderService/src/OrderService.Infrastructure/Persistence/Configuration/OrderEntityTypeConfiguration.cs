using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(o => o.SellerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.BuyerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.PaymentType)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired();

        builder.Property(o => o.ShippingCompanyId)
            .IsRequired();

        builder.Property(o => o.OrderAddressId)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.HasOne(o => o.ShippingCompany)
            .WithMany()
            .HasForeignKey(o => o.ShippingCompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Address)
            .WithOne()
            .HasForeignKey<Order>(o => o.OrderAddressId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Products)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}