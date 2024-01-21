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
            
            var ServicesTypesWithFullInfo = await _servicesTypeServices.GetAllServicesType();
            
            return Ok(ServicesTypesWithFullInfo);
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateServicesType(Servicetype registrationModel) {
           
            var result = await _servicesTypeServices.CreateServiceType(registrationModel);
           
            if (result is OkObjectResult okResult) {
                return Ok(okResult.Value);
            }  else if (result is BadRequestObjectResult badRequestObjectResult) {
                return BadRequest(badRequestObjectResult.Value);
            } 
            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("delete/{ServiceTypeId}")]
        public async Task<IActionResult> DeleteServiceTypesAsync(int ServiceTypeId) {
            
            var result = await _servicesTypeServices.DeleteService(ServiceTypeId);

            if (result is OkObjectResult okResult) {
            
                return Ok(okResult.Value);
            
            } else if (result is NotFoundObjectResult notFoundResult) {
            
                return NotFound(notFoundResult.Value);
            
            } else {
            
                return StatusCode(500, "Internal Server Error");
            
            }
        }

        [HttpPut("update/{ServiceTypeId}")]
        public async Task<IActionResult> UpdateServiceTypesAsync(int ServiceTypeId, Servicetype servicetype) {
           
            var result = await _servicesTypeServices.UpdateServiceTypeAsync(ServiceTypeId, servicetype);

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
