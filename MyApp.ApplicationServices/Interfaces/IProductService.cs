using MyApp.Domain.Dtos;

namespace MyApp.Domain.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateProductDto dto);
}
