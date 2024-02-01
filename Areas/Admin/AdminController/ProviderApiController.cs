using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Areas.Admin.AdminController {
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderApiController : Controller {
        private readonly ProviderSerivces _providerSerivices;
        private readonly DlctContext _dlctContext;
        public ProviderApiController(ProviderSerivces providerSerivces, DlctContext dlctContext) {
            _providerSerivices = providerSerivces;
            _dlctContext = dlctContext;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllProvider() {
            
            var providersWithFullInfo = await _providerSerivices.GetAllProvider();
            
            return Ok(providersWithFullInfo);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProvider(Provider registrationModel) {
           
            var result = await _providerSerivices.CreateProvider(registrationModel);
           
            if (result is OkObjectResult okResult) {

                return Ok(okResult.Value);
            
            }  else if (result is BadRequestObjectResult badRequestObjectResult) {
            
                return BadRequest(badRequestObjectResult.Value);
            
            } 
            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("delete/{providerId}")]
        public async Task<IActionResult> DeleteProductTypesAsync(int providerId) {
            
            var result = await _providerSerivices.DelteProvider(providerId);

            if (result is OkObjectResult okResult) {
            
                return Ok(okResult.Value);
            
            } else if (result is NotFoundObjectResult notFoundResult) {
            
                return NotFound(notFoundResult.Value);
            
            } else {
            
                return StatusCode(500, "Internal Server Error");
            
            }
        }

        [HttpDelete("deleteAll")]
        public async Task<IActionResult> DeleteProductTypessAsync([FromBody] List<int> providerId)
        {
            try
            {
                foreach (var ProviderId in providerId)
                {
                    var result = await _providerSerivices.DelteProvider(ProviderId);
                }
                

                var deleteSuccessResponse = new
                {
                    Message = "Provider deleted successfully",
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting Provider: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
         [HttpPut("update/{providerId}")]
        public async Task<IActionResult> UpdateProvider(int providerId, Provider updateModel) 
        {
            var result = await _providerSerivices.UpdateProvider(providerId, updateModel);

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
