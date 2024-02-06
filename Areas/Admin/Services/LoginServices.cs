// LoginService.cs
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public class LoginService
    {
        private readonly DlctContext _dlctContext;
  
        private readonly GenerateRandomKey _generateRandomKey;

        public LoginService(DlctContext dlctContext, GenerateRandomKey ganerateRandomKey)
        {
            _dlctContext = dlctContext;
            _generateRandomKey = ganerateRandomKey;
        }

        public async Task<IActionResult> RegisterClient(Staff registrationModel)
        {
            if (registrationModel == null)
            {
                return new BadRequestObjectResult("Registration data is empty.");
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
                return new BadRequestObjectResult(emptyFieldsErrorResponse);
            }

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
            return new OkObjectResult(registrationSuccessResponse);
        }

         public async Task<IActionResult> Login(Staff loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Username) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                var errorResponse = new
                {
                    Message = "Username and password cannot be empty"
                };
                return new BadRequestObjectResult(errorResponse);
            }

            var staff = await _dlctContext.Staff.FirstOrDefaultAsync(c => c.Username == loginModel.Username);

            if (staff == null)
            {
                var loginErrorResponse = new
                {
                    Message = "Invalid username or password",
                };
                return new BadRequestObjectResult(loginErrorResponse);
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginModel.Password, staff.Password);

            if (isPasswordValid)
            {
                string token =  _generateRandomKey.CreateToken(staff);
                
                var loginSuccessResponse = new
                {
                    Token = token, 
                    Message = "Login successful"
                };

                return new OkObjectResult(token);
            }

            var invalidLoginErrorResponse = new
            {
                Message = "Invalid username or password",
                Errors = new List<string>
                {
                    "Invalid password"
                }
            };

            return new BadRequestObjectResult(invalidLoginErrorResponse);
        }
    }
}
