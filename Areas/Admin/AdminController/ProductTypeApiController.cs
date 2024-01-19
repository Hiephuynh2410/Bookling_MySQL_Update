using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Areas.Admin.AdminController {
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypeApiController : Controller {
        private readonly ProductTypeSevices _productTypeSevices;

        public ProductTypeApiController(ProductTypeSevices productTypeSevices) {
            _productTypeSevices = productTypeSevices;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllProductType() {
            
            var productTypesWithFullInfo = await _productTypeSevices.GetAllProductTypes();
            
            return Ok(productTypesWithFullInfo);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProductsType(Producttype registrationModel) {
           
            var result = await _productTypeSevices.CreateProductType(registrationModel);
           
            if (result is OkObjectResult okResult) {
                return Ok(okResult.Value);
            }  else if (result is BadRequestObjectResult badRequestObjectResult) {
                return BadRequest(badRequestObjectResult.Value);
            } 
            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("delete/{productTypeId}")]
        public async Task<IActionResult> DeleteProductTypesAsync(int productTypeId) {
            
            var result = await _productTypeSevices.DeleteProductType(productTypeId);

            if (result is OkObjectResult okResult) {
            
                return Ok(okResult.Value);
            
            } else if (result is NotFoundObjectResult notFoundResult) {
            
                return NotFound(notFoundResult.Value);
            
            } else {
            
                return StatusCode(500, "Internal Server Error");
            
            }
        }
    }
}
