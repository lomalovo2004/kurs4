using Microsoft.EntityFrameworkCore;
using Part2_Repository.Data;
using Part2_Repository.Models;
using Part2_Repository.Repositories;
using Part2_Repository.Services;

namespace Part2_Repository;

internal static class Program
{
    private static string GetConnectionString()
    {
        return Environment.GetEnvironmentVariable("DELIVERY_DB_CONNECTION")
            ?? "server=localhost;port=3306;database=delivery_db;user=root;password=1234;";
    }

    public static async Task Main()
    {
        Console.WriteLine("=== Лабораторная работа №2. Часть 2: Repository ===");

        var options = new DbContextOptionsBuilder<DeliveryDbContext>()
            .UseMySql(GetConnectionString(), ServerVersion.AutoDetect(GetConnectionString()))
            .Options;

        await using var context = new DeliveryDbContext(options);

        try
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(
                    new Category { Name = "Электроника" },
                    new Category { Name = "Товары для дома" });
                await context.SaveChangesAsync();
            }

            var electronicsId = await context.Categories
                .Where(x => x.Name == "Электроника")
                .Select(x => x.Id)
                .FirstAsync();

            var productRepository = new ProductRepository(context);

            var product = new Product
            {
                Name = "Беспроводные наушники",
                Price = 5990m,
                StockQuantity = 10,
                CategoryId = electronicsId
            };

            // CREATE
            productRepository.Add(product);
            await context.SaveChangesAsync();
            Console.WriteLine($"CREATE: добавлен товар #{product.Id} — {product.Name}");

            // READ
            var found = productRepository.GetById(product.Id);
            Console.WriteLine($"READ: {found?.Name}, цена {found?.Price:0.00} руб.");

            // UPDATE
            if (found is not null)
            {
                found.Price = 5790m;
                productRepository.Update(found);
                await context.SaveChangesAsync();
                Console.WriteLine($"UPDATE: новая цена {found.Price:0.00} руб.");
            }

            // Specialized repository
            var inStock = productRepository.GetInStock();
            Console.WriteLine($"GetInStock: найдено товаров в наличии — {inStock.Count}");

            // DELETE
            if (found is not null)
            {
                productRepository.Delete(found);
                await context.SaveChangesAsync();
                Console.WriteLine($"DELETE: товар #{found.Id} удалён.");
            }

            // Generic repository + Unit of Work
            using var unitOfWork = new UnitOfWork(
                new DeliveryDbContext(
                    new DbContextOptionsBuilder<DeliveryDbContext>()
                        .UseMySql(GetConnectionString(), ServerVersion.AutoDetect(GetConnectionString()))
                        .Options));

            unitOfWork.Customers.Add(new Customer
            {
                FullName = "Иванов Иван Иванович",
                Phone = "+7-900-000-00-01",
                Email = "ivanov@example.com"
            });
            unitOfWork.SaveChanges();

            Console.WriteLine($"Unit of Work: клиентов в БД — {unitOfWork.Customers.GetAll().Count}");

            // Async repository
            var asyncRepository = new AsyncRepository<Category>(context);
            var categories = await asyncRepository.GetAllAsync();
            Console.WriteLine($"AsyncRepository: категорий — {categories.Count}");

            Console.WriteLine("=== Часть 2 завершена ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Не удалось подключиться к MySQL.");
            Console.WriteLine($"Причина: {ex.Message}");
            Console.WriteLine("Проверьте строку подключения и запущенный сервер MySQL.");
        }
    }
}
