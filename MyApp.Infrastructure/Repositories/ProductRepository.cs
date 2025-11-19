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
            .Where(p => p.UnitPrice >= minPrice && p.UnitPrice <= maxPrice)
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
        using DbConnection connection = (SqlConnection)_context.Database.GetDbConnection();
        var sql = @"SELECT ProductID, ProductName, UnitPrice, p.CategoryID, CategoryName
                    FROM Products p 
                    INNER JOIN Categories c ON p.CategoryID = c.CategoryID";

        return await connection.QueryAsync<Product, Category, Product>(sql, (product, category) => {
            product.Category = category;
            return product;
        },
        splitOn: "CategoryID");
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        using DbConnection connection = (SqlConnection)_context.Database.GetDbConnection();
        var sql = @"SELECT *
                    FROM Products";

        return await connection.QueryAsync<Product>(sql);
    }
}
