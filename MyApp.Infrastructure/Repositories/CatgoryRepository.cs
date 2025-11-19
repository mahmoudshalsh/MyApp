using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Dtos;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces.Repositories;
using System.Data;
using System.Data.Common;

namespace MyApp.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext appDbContext) : GenericRepository<Category, AppDbContext>(appDbContext), ICategoryRepository
{
    public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
    {
        return await _dbSet
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.CategoryID == categoryId);
    }

    public async Task<IEnumerable<SalesCategory>> GetAllSalesByCategory(string CategoryName)
    {
        using DbConnection connection = (SqlConnection)_context.Database.GetDbConnection();
        var sql = @"[dbo].[SalesByCategory]";
        var parameters = new DynamicParameters();
        parameters.Add("@CategoryName", CategoryName);
        return await connection.QueryAsync<SalesCategory>(sql, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<DateTime?> GetToday()
    {
        using DbConnection connection = (SqlConnection)_context.Database.GetDbConnection();
        var sql = @"SELECT [dbo].[GetToday]()";
        return await connection.ExecuteScalarAsync<DateTime?>(sql);
    }
}