using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configuration;

public class OrderProductEntityTypeConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("OrderProducts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Quantity).IsRequired();
        builder.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();

        builder.HasOne(p => p.Product)
               .WithMany()
               .HasForeignKey(p => p.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Order)
               .WithMany(o => o.Products)
               .HasForeignKey(p => p.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
