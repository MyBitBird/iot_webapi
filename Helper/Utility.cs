using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace IOT.Helper
{
    public static class Utility
    {
        public static string BuildToken(Models.Users user, IConfiguration config,MyEnums.UserTypes type)
        {
            var claims = new[] {
                new Claim("Name", user.Name),
                new Claim("Status", user.Status.ToString()),
                new Claim("ID", user.Id.ToString()),
                new Claim(ClaimTypes.Role,  type == MyEnums.UserTypes.Admin ? "ADMIN" : "DEVICE")
                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static Guid GetCurrentUserId(ClaimsPrincipal user)
        {
            return user == null ? Guid.Empty : Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == "ID")?.Value );
        }
        public static string GetCurrentUserRole(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }

        public static string HashPassword(String password,IConfiguration config)
        {
            byte[] saltByte = Encoding.UTF8.GetBytes(config["Salt:key"]);

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltByte,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
            
        }
    }
}