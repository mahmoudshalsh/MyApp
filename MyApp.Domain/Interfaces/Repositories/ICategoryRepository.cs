using MyApp.Domain.Entities;

namespace MyApp.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryWithProductsAsync(int categoryId);
}
