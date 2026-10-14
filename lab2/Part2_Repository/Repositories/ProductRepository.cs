using Microsoft.EntityFrameworkCore;
using Part2_Repository.Data;
using Part2_Repository.Models;

namespace Part2_Repository.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(DeliveryDbContext context) : base(context)
    {
    }

    public IReadOnlyList<Product> GetByCategory(int categoryId) =>
        DbSet.AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Name)
            .ToList();

    public IReadOnlyList<Product> GetInStock() =>
        DbSet.AsNoTracking()
            .Where(x => x.StockQuantity > 0)
            .OrderBy(x => x.Name)
            .ToList();
}
