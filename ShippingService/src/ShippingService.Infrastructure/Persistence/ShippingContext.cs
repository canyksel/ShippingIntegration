using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ShippingService.Domain.Common;
using ShippingService.Domain.Common.Interfaces;
using ShippingService.Domain.Entities;
using ShippingService.Infrastructure.Persistence.Configuration;

namespace ShippingService.Infrastructure.Persistence;

public class ShippingContext(
    DbContextOptions<ShippingContext> options,
    IDomainEventDispatcher domainEventDispatcher)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Shipment> Shipments { get; set; }
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await Database.BeginTransactionAsync();
    }

    public async Task ComitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        await transaction.CommitAsync();
    }

    public async Task<bool> SaveEntitesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);

        var domainEntities = ChangeTracker
            .Entries<EntityBase<Guid>>()
            .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents!)
            .ToList();

        foreach (var entity in domainEntities)
        {
            entity.Entity.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            await domainEventDispatcher.DispatchAsync(domainEvent, cancellationToken);
        }

        return true;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShipmentEntityTypeConfiguration());
    }
}