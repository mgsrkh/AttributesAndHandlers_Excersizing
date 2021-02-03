using AttributesAndHandlers.IJWTAuthentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AttributesAndHandlers.JWTAuthentication
{
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly string tokenKey;
        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }

        IDictionary<string, string> users = new Dictionary<string, string>
        {
            { "Get", "password1" }, 
            //{ "test2", "password2" }
        };
        public string Authenticate(string username, string password) 
        {

            if (!users.Any(u => u.Key == username && u.Value == password))
            {
                return null;
            }

            var routeList = new List<string>() { "Get", "Post", "Put", "Patch" };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   // new Claim("Get",username)
                   new Claim("Routes", routeList[0]),
                   new Claim("DateOfJoining","1398-02-02")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
