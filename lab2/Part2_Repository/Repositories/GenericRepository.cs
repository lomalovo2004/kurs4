using Microsoft.EntityFrameworkCore;
using Part2_Repository.Data;

namespace Part2_Repository.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly DeliveryDbContext Context;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(DeliveryDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public T? GetById(int id) => DbSet.Find(id);

    public IReadOnlyList<T> GetAll() => DbSet.AsNoTracking().ToList();

    public void Add(T entity) => DbSet.Add(entity);

    public void Update(T entity) => DbSet.Update(entity);

    public void Delete(T entity) => DbSet.Remove(entity);
}
