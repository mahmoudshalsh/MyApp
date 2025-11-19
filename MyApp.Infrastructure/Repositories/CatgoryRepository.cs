using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces.Repositories;

namespace MyApp.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext appDbContext) : GenericRepository<Category, AppDbContext>(appDbContext), ICategoryRepository
{
    public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
    {
        return await _dbSet
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
    }
}