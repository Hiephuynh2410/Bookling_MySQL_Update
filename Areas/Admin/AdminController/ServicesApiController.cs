using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Areas.Admin.AdminController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesApiController : Controller
    {
        private readonly ServiceServices _serviceServices;

        public ServicesApiController(ServiceServices serviceServices)
        {
            _serviceServices = serviceServices;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllServices()
        {
            var ServicesWithFullInfo = await _serviceServices.GetAllServices();
            return Ok(ServicesWithFullInfo);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateServicesAsync(Service registrationModel) 
        {
            var result = await _serviceServices.CreateServiceAsync(registrationModel);

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

        [HttpDelete("delete/{ServiceId}")]
        public async Task<IActionResult> DeleteServicesAsync(int ServiceId) 
        {
            var result = await _serviceServices.DeleteServiceAsync(ServiceId);

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

        [HttpDelete("deleteAll")]
        public async Task<IActionResult> DeleteProductsAsync([FromBody] List<int> ServiceIds)
        {
            try
            { 
                foreach (var serviceId in ServiceIds)
                {
                    var result = await _serviceServices.DeleteAllServiceAsync(serviceId);
                }

                var deleteSuccessResponse = new
                {
                    Message = "Service deleted successfully"
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.Error.WriteLine($"Error deleting Service: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
