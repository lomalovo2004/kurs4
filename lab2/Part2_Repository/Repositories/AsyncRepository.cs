using Microsoft.EntityFrameworkCore;
using Part2_Repository.Data;

namespace Part2_Repository.Repositories;

public class AsyncRepository<T> where T : class
{
    private readonly DeliveryDbContext _context;
    private readonly DbSet<T> _dbSet;

    public AsyncRepository(DeliveryDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _dbSet.FindAsync(new object[] { id }, cancellationToken).AsTask();

    public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
