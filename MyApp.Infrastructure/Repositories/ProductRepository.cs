using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces.Repositories;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace MyApp.Infrastructure.Repositories;

public class ProductRepository(AppDbContext appDbContext) : GenericRepository<Product, AppDbContext>(appDbContext), IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _dbSet
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .ToListAsync();
    }

    public async IAsyncEnumerable<Product> StreamAllAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var products = await _dbSet.ToListAsync(cancellationToken);
        foreach (var product in products)
        {
            // do events
            await Task.Delay(10_000, cancellationToken); // Simulate some processing delay
            yield return product;
        }
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        DbConnection connection = (SqlConnection)_context.Database.GetDbConnection();
        return await connection.QueryAsync<Product>("SELECT * FROM Products");
    }
}
