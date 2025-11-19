using Microsoft.AspNetCore.Mvc;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Interfaces.Repositories;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(IAppUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("WithProducts")]
    public async Task<IActionResult> GetCategoryWithProducts(int categoryId)
    {
        var categories = await _categoryRepository.GetCategoryWithProductsAsync(categoryId);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _unitOfWork.Repository<Category>().AddAsync(category);
        await _unitOfWork.SaveAsync();
        return CreatedAtAction(nameof(Create), new { id = category.CategoryID }, category);
    }

    [HttpGet("SalesByCategory")]
    public async Task<IActionResult> GetAllSalesByCategory([FromQuery]string categoryName)
    {
        var salesCategories = await _categoryRepository.GetAllSalesByCategory(categoryName);
        return Ok(salesCategories);
    }

    [HttpGet("Today")]
    public async Task<IActionResult> GetToday()
    {
        var today = await _categoryRepository.GetToday();
        return Ok(today?.Date);
    }
}