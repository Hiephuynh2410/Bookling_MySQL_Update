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
            var servicesWithFullInfo = await _serviceServices.GetAllServicesWithFullInfoAsync();
            return Ok(servicesWithFullInfo);
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
    
        [HttpPut("update/{serviceId}")]
        public async Task<IActionResult> UpdateServicesAsync(int serviceId, Service updateModel) 
        {
            var result = await _serviceServices.UpdateServiceAsync(serviceId, updateModel);

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

        [HttpDelete("delete/{serviceId}")]
        public async Task<IActionResult> DeleteServicesAsync(int serviceId) 
        {
            var result = await _serviceServices.DeleteServiceAsync(serviceId);

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
        public async Task<IActionResult> DeleteAllServicesAsync([FromBody] List<int> serviceId)
        {
            try
            {
                foreach (var ServiceId in serviceId)
                {
                    var result = await _serviceServices.DeleteServiceAsync(ServiceId);
                }

                var deleteSuccessResponse = new
                {
                    Message = "service deleted successfully"
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting service: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
