using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace IOT.Helper
{
    public static class Utility
    {

        public static string BuildToken(Models.Users user, IConfiguration _config,String role)
        {
            var claims = new[] {
                new Claim("Name", user.Name),
                new Claim("Status", user.Status.ToString()),
                new Claim("ID", user.Id.ToString()),
                new Claim(ClaimTypes.Role, role),//ADMIN,DEVICE
                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static Guid GetCurrentUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

        }
        public static string GetCurrentUserRole(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;

        }

        public static String HashPassword(String password,IConfiguration config)
        {
            byte[] saltByte = Encoding.UTF8.GetBytes(config["Salt:key"]);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltByte,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
            
        }





    }
}