using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;

namespace WebApi.Helpers
{
    public static class JWTHelper
    {
        public static string KEY = "179d3a7c1a8ed4ae1d7d375ce1411730ce869f26a14a9cf68bd5de678ba9e14174c6eb8653525a005ba2c422a6005bedace17132853b5cf527b82580fec5dd3f";
        public static string Generate(int userId)
        {
            //init claims payload
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, userId.ToString()),
            };

            //set jwt config
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)), SecurityAlgorithms.HmacSha512)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
