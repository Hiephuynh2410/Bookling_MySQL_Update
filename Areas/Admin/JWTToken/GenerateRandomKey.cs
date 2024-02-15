using Booking.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Booking.Services
{
    public class GenerateRandomKey
    {   
        private string GenerateRandomKeys(int length)
        {
            var random = new RNGCryptoServiceProvider();
            byte[] keyBytes = new byte[length / 8];
            random.GetBytes(keyBytes);

            return Convert.ToBase64String(keyBytes);
        }

        public string CreateToken(Staff staff)
        {
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, staff.Username ?? string.Empty),
                new Claim(ClaimTypes.Email, staff.Email ?? string.Empty),
                new Claim(ClaimTypes.Email, staff.Password ?? string.Empty),
                new Claim(ClaimTypes.Email, staff.Email ?? string.Empty),
                new Claim(ClaimTypes.Email, staff.Address ?? string.Empty),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GenerateRandomKeys(512)));

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