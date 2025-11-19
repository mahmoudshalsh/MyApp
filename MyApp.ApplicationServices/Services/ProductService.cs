using MyApp.ApplicationServices.Dtos;
using MyApp.ApplicationServices.Interfaces;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Interfaces.Repositories;

namespace MyApp.ApplicationServices.Services;

public class ProductService : IProductService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public ProductService(IAppUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => new ProductDto(p.Id, p.Name, p.Price, null));
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product is null ? null : new ProductDto(product.Id, product.Name, product.Price, null);
    }

    public async Task CreateAsync(CreateProductDto dto)
    {
        Product product = new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };
        await _productRepository.AddAsync(product);
        await _unitOfWork.SaveAsync();
    }

}
