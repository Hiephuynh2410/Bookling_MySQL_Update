using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Booking.Services;

namespace Booking.Areas.Admin.AdminController
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginApiController : ControllerBase
    {
        private readonly DlctContext _dlctContext;

        private readonly LoginService _loginService;


        public LoginApiController(DlctContext dlctContext,  LoginService loginService)
        {
            _dlctContext = dlctContext;
            _loginService = loginService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult>RegisterStaffAsync(Staff registrationModel) 
        {
            var result = await _loginService.RegisterClient(registrationModel);

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

        [HttpPost("login")]
        public async Task<IActionResult>LoginAsync(Staff loginModel) 
        {
            var result = await _loginService.Login(loginModel);

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
    }
}
