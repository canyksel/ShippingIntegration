using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Common.Interfaces;
using OrderService.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Repositories.Common;

public class EfRepository<T>(OrderContext context) : IRepository<T> where T : class
{
    protected readonly OrderContext _context = context ?? throw new ArgumentNullException(nameof(context));
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
