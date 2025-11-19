using MyApp.Domain.Dtos;
using MyApp.Domain.Entities;

namespace MyApp.Domain.Interfaces.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<SalesCategory>> GetAllSalesByCategory(string CategoryName);
    Task<Category?> GetCategoryWithProductsAsync(int categoryId);
    Task<DateTime?> GetToday();
}
