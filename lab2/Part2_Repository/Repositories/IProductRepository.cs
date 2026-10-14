using Part2_Repository.Models;

namespace Part2_Repository.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IReadOnlyList<Product> GetByCategory(int categoryId);
    IReadOnlyList<Product> GetInStock();
}
