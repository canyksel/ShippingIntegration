using Microsoft.EntityFrameworkCore;
using ShippingService.Domain.Common.Interfaces;
using ShippingService.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace ShippingService.Infrastructure.Repositories.Common;

public class EfRepository<T>(ShippingContext context) : IRepository<T> where T : class
{
    protected readonly ShippingContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public IUnitOfWork UnitOfWork => _context;

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AnyAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}