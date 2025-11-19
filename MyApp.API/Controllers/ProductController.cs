using Microsoft.AspNetCore.Mvc;
using MyApp.ApplicationServices.Dtos;
using MyApp.ApplicationServices.Interfaces;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all products");

        var products = await _productService.GetAllAsync();

        _logger.LogInformation("Fetched {Count} products", products.Count());

        return Ok(products);
    }

    //[HttpGet("PriceRange")]
    //public async Task<IActionResult> GetProductsByPriceRangeAsync([FromQuery] decimal min, [FromQuery] decimal max)
    //{
    //    var products = await _productService.GetProductsByPriceRangeAsync(min, max);
    //    return Ok(products);
    //}

    //[HttpGet("stream")]
    //public async IAsyncEnumerable<Product> StreamAllProducts([EnumeratorCancellation] CancellationToken cancellationToken = default)
    //{
    //    await foreach (var product in _productRepository.StreamAllAsync(cancellationToken))
    //    {
    //        yield return product;
    //    }
    //}


    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto product)
    {
        await _productService.CreateAsync(product);
        return Ok(new { message = "Product created successfully" });
    }


    //[HttpPost("bulk-create")]
    //public async Task<IActionResult> BulkCreate([FromBody] List<Product> products)
    //{
    //    await _unitOfWork.BeginTransactionAsync();
    //    try
    //    {
    //        foreach (var product in products)
    //        {
    //            await _unitOfWork.Repository<Product>().AddAsync(product);
    //        }

    //        await _unitOfWork.SaveChangesAsync();
    //        await _unitOfWork.CommitTransactionAsync();

    //        return Ok(new { message = "Products created successfully" });
    //    }
    //    catch
    //    {
    //        await _unitOfWork.RollbackTransactionAsync();
    //        return StatusCode(500, "Transaction rolled back due to an error");
    //    }
    //}
}