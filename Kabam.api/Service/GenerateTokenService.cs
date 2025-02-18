
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

            var secretKey = Secret.Key;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Email)
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
