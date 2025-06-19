using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Domain.Common;
using OrderService.Domain.Common.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Persistence;

public class OrderContext(
    DbContextOptions<OrderContext> options,
    IDomainEventDispatcher domainEventDispatcher)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderAddress> OrderAddresses { get; set; }
    public DbSet<CompanyAddress> CompanyAddresses { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<ShippingCompany> ShippingCompanies { get; set; }

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
}