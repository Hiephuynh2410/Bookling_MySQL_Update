using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Booking.Areas.Admin.AdminController
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginApiController : ControllerBase
    {
        private readonly DlctContext _dlctContext;
        private readonly IConfiguration _configuration;

        public LoginApiController(DlctContext dlctContext, IConfiguration configuration)
        {
            _dlctContext = dlctContext;
            _configuration = configuration;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterClient(Staff registrationModel)
        {
            if (registrationModel == null)
            {
                return BadRequest("Registration data is empty.");
            }

            if (string.IsNullOrWhiteSpace(registrationModel.Username) ||
                string.IsNullOrWhiteSpace(registrationModel.Password) ||
                string.IsNullOrWhiteSpace(registrationModel.Name) ||
                string.IsNullOrWhiteSpace(registrationModel.Phone) ||
                string.IsNullOrWhiteSpace(registrationModel.Email))
            {
                var emptyFieldsErrorResponse = new
                {
                    Message = "Không được để trống username password name phone Email đâu Cưng!",
                };
                return BadRequest(emptyFieldsErrorResponse);
            }


            if (ModelState.IsValid)
            {
                var createdStaff = await _dlctContext.Staff
                    .Include(s => s.Role)
                    .FirstOrDefaultAsync(p => p.StaffId == registrationModel.StaffId || p.Username == registrationModel.Username);

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationModel.Password);

                var newStaff = new Staff
                {
                    Name = registrationModel.Name,
                    Username = registrationModel.Username,
                    Password = hashedPassword,
                    Phone = registrationModel.Phone,
                    Address = registrationModel.Address,
                    Avatar = registrationModel.Avatar,
                    Email = registrationModel.Email,
                    Status = registrationModel.Status,
                    CreatedAt = DateTime.Now,
                    CreatedBy = registrationModel.CreatedBy,
                    Role = createdStaff?.Role,
                };

                _dlctContext.Staff.Add(newStaff);
                await _dlctContext.SaveChangesAsync();


                _dlctContext.Entry(newStaff).Reference(c => c.Role).Load();

                var registrationSuccessResponse = new
                {
                    Message = "Registration successful",
                    ClientId = newStaff.StaffId,
                    Role = new
                    {
                        Name = newStaff.Role?.Name,
                        RoleId = newStaff.Role?.RoleId
                    }
                };
                return Ok(registrationSuccessResponse);
            }

            var invalidDataErrorResponse = new
            {
                Message = "Invalid registration data",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            };
            return BadRequest(invalidDataErrorResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Staff loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Username) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                var errorResponse = new
                {
                    Message = "Username and password cannot be empty"
                };
                return BadRequest(errorResponse);
            }

            var staff = await _dlctContext.Staff.FirstOrDefaultAsync(c => c.Username == loginModel.Username);

            if (staff == null)
            {
                var loginErrorResponse = new
                {
                    Message = "Invalid username or password",
                };
                return BadRequest(loginErrorResponse);
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginModel.Password, staff.Password);

            if (isPasswordValid)
            {
                string token = CreateToken(staff);
                
                var loginSuccessResponse = new
                {
                    Token = token, 
                    userID = staff.StaffId,
                    Username = staff.Username,
                    Name = staff.Name,
                    phone = staff.Phone,
                    Address = staff.Address,
                    Email = staff.Email,
                    Message = "Login successful"
                };

                return Ok(token);
            }

            var invalidLoginErrorResponse = new
            {
                Message = "Invalid username or password",
                Errors = new List<string>
                {
                    "Invalid password"
                }
            };

            return BadRequest(invalidLoginErrorResponse);
        }

        private string GenerateRandomKey(int length)
        {
            var random = new RNGCryptoServiceProvider();
            byte[] keyBytes = new byte[length / 8];
            random.GetBytes(keyBytes);

            return Convert.ToBase64String(keyBytes);
        }


        private string CreateToken(Staff staff) 
        {
            List<Claim> claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, staff.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GenerateRandomKey(512)));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
