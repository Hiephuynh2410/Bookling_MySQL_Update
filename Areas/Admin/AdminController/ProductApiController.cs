using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Areas.Admin.AdminController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : Controller
    {
        private readonly ProductService _productService;

        public ProductApiController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllProducts()
        {
            var productsWithFullInfo = await _productService.GetAllProductsWithFullInfoAsync();
            return Ok(productsWithFullInfo);
        }
        
       [HttpPost("create")]
        public async Task<IActionResult> CreateProductsAsync(Product registrationModel) 
        {
            var result = await _productService.CreateProductAsync(registrationModel);

            if (result is OkObjectResult okResult)
            {
                return Ok(okResult.Value);
            }
            else if (result is BadRequestObjectResult badRequestResult)
            {
                return BadRequest(badRequestResult.Value);
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> DeleteProductsAsync(int productId) 
        {
            var result = await _productService.DeleteProductAsync(productId);

            if (result is OkObjectResult okResult)
            {
                return Ok(okResult.Value);
            }
            else if (result is NotFoundObjectResult notFoundResult)
            {
                return NotFound(notFoundResult.Value);
            }
            else
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("delete/{productId}")]
        public async Task<IActionResult> UpdateProductsAsync(int productId, Product updateModel) 
        {
            var result = await _productService.UpdateProductAsync(productId, updateModel);

            if (result is OkObjectResult okResult)
            {
                return Ok(okResult.Value);
            }
            else if (result is NotFoundObjectResult notFoundResult)
            {
                return NotFound(notFoundResult.Value);
            }
            else
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
