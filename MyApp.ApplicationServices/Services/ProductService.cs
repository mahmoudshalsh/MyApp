using MyApp.Domain.Dtos;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Interfaces.Repositories;

namespace MyApp.Domain.Services;

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
        return products.Select(p => new ProductDto(p.ProductID, p.ProductName, p.UnitPrice, p.Category?.CategoryName));
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product is null ? null : new ProductDto(product.ProductID, product.ProductName, product.UnitPrice, null);
    }

    public async Task CreateAsync(CreateProductDto dto)
    {
        Product product = new()
        {
            ProductID = dto.Id,
            ProductName = dto.Name,
            UnitPrice = dto.Price,
            CategoryID = dto.CategoryId
        };
        await _productRepository.AddAsync(product);
        await _unitOfWork.SaveAsync();
    }

}
