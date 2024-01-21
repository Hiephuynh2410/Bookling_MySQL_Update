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
    }
}
