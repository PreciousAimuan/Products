using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Domain.Models;
using Products.Infrastructure.Services;

namespace Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
