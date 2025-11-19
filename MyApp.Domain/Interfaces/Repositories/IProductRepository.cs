using MyApp.Domain.Entities;

namespace MyApp.Domain.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    IAsyncEnumerable<Product> StreamAllAsync(CancellationToken cancellationToken);
}
