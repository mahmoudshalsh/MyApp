using MyApp.ApplicationServices.Dtos;

namespace MyApp.ApplicationServices.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateProductDto dto);
}
