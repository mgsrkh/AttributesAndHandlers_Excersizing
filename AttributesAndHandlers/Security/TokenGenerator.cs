using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttributesAndHandlers.Security
{
    public class TokenGenerator
    {
        public static string GenerateEncodedToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim("IsDeveloper", "true"), //custom Claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mgsrkh$%^UU8(@#"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "www.mft.com",
                audience: "www.mft.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(2000),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
