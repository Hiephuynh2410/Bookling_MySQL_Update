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
    }
}
