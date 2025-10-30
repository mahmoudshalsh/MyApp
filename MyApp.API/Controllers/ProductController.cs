using Microsoft.AspNetCore.Mvc;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;

namespace MyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }


        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate([FromBody] List<Product> products)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var product in products)
                {
                    await _unitOfWork.Repository<Product>().AddAsync(product);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Ok(new { message = "Products created successfully" });
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return StatusCode(500, "Transaction rolled back due to an error");
            }
        }
    }
}