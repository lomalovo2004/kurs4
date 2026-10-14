using Part2_Repository.Data;
using Part2_Repository.Models;
using Part2_Repository.Repositories;

namespace Part2_Repository.Services;

public sealed class UnitOfWork : IDisposable
{
    private readonly DeliveryDbContext _context;

    public IProductRepository Products { get; }

    public IRepository<Customer> Customers { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Order> Orders { get; }

    public UnitOfWork(DeliveryDbContext context)
    {
        _context = context;
        Products = new ProductRepository(context);
        Customers = new GenericRepository<Customer>(context);
        Categories = new GenericRepository<Category>(context);
        Orders = new GenericRepository<Order>(context);
    }

    public int SaveChanges() => _context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
