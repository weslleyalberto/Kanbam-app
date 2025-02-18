
using Kabam.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kabam.api.Service
{
    public class GenerateTokenService
    {
        public static string GenerateJwtToken(User user)
        {

            var secretKey = "d3854a41-b72d-4fb4-b418-189aeeaeafaed3854a41-b72d-4fb4-b418-189aeeaeafae";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Id)
        };

            var token = new JwtSecurityToken(
               
                claims: claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
