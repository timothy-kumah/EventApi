using EventApi.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventApi
{
    public static class AuthHandler
    {

        public static string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim("cogname",user.CogNome),
                new Claim("email",user.Email),
                new Claim("role",user.Role),
                new Claim("nome",user.Nome),  
            };

            var token = new JwtSecurityToken("https://localhost:7018/",
                "https://localhost:7018",
                claims,
                expires: DateTime.UtcNow.AddMinutes(180),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
