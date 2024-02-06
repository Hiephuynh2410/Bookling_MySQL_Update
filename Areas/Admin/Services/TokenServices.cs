// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.IdentityModel.Tokens;

// public class TokenService
// {
//     private readonly string _secretKey;

//     public TokenService(string secretKey)
//     {
//         _secretKey = secretKey;
//     }

//     public string GenerateToken(string username)
//     {
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var key = Encoding.UTF8.GetBytes(_secretKey);
//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(new Claim[]
//             {
//                 new Claim(ClaimTypes.Name, username)
//             }),
//             Expires = DateTime.UtcNow.AddHours(1),
//             SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//         };

//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         return tokenHandler.WriteToken(token);
//     }
// }
