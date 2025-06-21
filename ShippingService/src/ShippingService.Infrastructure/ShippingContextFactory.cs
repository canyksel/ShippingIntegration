using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ShippingService.Domain.Common.Interfaces;
using ShippingService.Infrastructure.Persistence;

namespace ShippingService.Infrastructure;

public class ShippingContextFactory : IDesignTimeDbContextFactory<ShippingContext>
{
    public ShippingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShippingContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=shippingdb;Username=postgresshippingdb;Password=ShippingDb12345.");

        return new ShippingContext(optionsBuilder.Options, new NoOpDomainEventDispatcher());
    }

    private class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
