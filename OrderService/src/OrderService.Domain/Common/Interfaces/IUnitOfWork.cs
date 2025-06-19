using Microsoft.EntityFrameworkCore.Storage;

namespace OrderService.Domain.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task ComitTransactionAsync(IDbContextTransaction transaction);
}
