using System.Linq.Expressions;

namespace ShippingService.Domain.Common.Interfaces;

public interface IRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }

    Task<T> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> ListAsync();
    Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
