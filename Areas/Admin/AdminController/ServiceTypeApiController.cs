using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Areas.Admin.AdminController {
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTypeApiController : Controller {
        private readonly ServicesTypeServices _servicesTypeServices;

        public ServiceTypeApiController(ServicesTypeServices servicesTypeServices) {
            _servicesTypeServices = servicesTypeServices;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllServicesType() {
            
            var ServicesTypesWithFullInfo = await _servicesTypeServices.GetAllServicesTypes();
            
            return Ok(ServicesTypesWithFullInfo);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateServicesType(Servicetype registrationModel) {
           
            var result = await _servicesTypeServices.CreateServicesType(registrationModel);
           
            if (result is OkObjectResult okResult) {
                return Ok(okResult.Value);
            }  else if (result is BadRequestObjectResult badRequestObjectResult) {
                return BadRequest(badRequestObjectResult.Value);
            } 
            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("delete/{serviceTypeId}")]
        public async Task<IActionResult> DeleteServiceTypesAsync(int serviceTypeId) {
            
            var result = await _servicesTypeServices.DeleteServicesType(serviceTypeId);

            if (result is OkObjectResult okResult) {
            
                return Ok(okResult.Value);
            
            } else if (result is NotFoundObjectResult notFoundResult) {
            
                return NotFound(notFoundResult.Value);
            
            } else {
            
                return StatusCode(500, "Internal Server Error");
            
            }
        }

        [HttpDelete("deleteAll")]
        public async Task<IActionResult> DeleteAllServiceTypesAsync([FromBody] List<int> serviceTypeId)
        {
            try
            {
                foreach (var ServiceTypeId in serviceTypeId)
                {
                    var result = await _servicesTypeServices.DeleteServicesType(ServiceTypeId);
                }

                var deleteSuccessResponse = new
                {
                    Message = "serviceType deleted successfully"
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.Error.WriteLine($"Error deleting serviceType: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        [HttpPut("update/{serviceTypeId}")]
        public async Task<IActionResult> UpdateServiceTypesAsync(int serviceTypeId, Servicetype servicetype) {
           
            var result = await _servicesTypeServices.UpdateServicesType(serviceTypeId, servicetype);

            if(result is OkObjectResult okResult) {

                return Ok(okResult.Value);

            } else if (result is NotFoundObjectResult notFoundResult) { 
              
                return NotFound(notFoundResult.Value);
            
            } else {
            
                return StatusCode(500, "Internal Server Error");
            
            }
        }
    }
}
