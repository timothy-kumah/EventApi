using EventApi.Data;
using EventApi.Dtos;
using EventApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EventContext _context;
        public AuthController(EventContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            try
            {
                var user = new User
                {
                    CogNome = dto.CogNome,
                    Email = dto.Email,
                    Nome = dto.Nome,
                    Password = dto.Password,
                    Role = dto.Role
                };

                _context.AddAsync(user);
                _context.SaveChanges();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Email already exists");
            } 
        }

        

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto) {
            if (dto != null)
            {
                var user =  _context.User.Where(e => e.Email == dto.Email && e.Password == dto.Password).FirstOrDefault();
                if (user!=null)
                {
                    var token = AuthHandler.GenerateToken(user);
                    return Ok(token);
                }
                return Unauthorized("Wrong Credentials");
            }
            return BadRequest("Invalid Email or Paswword");
        }

        
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.User.Where(e => e.Email == email).FirstOrDefault();
            if (user!=null)
            {
                return Ok(AuthHandler.GenerateToken(user));
            }
            return Unauthorized("Email doesnt exists");
        }

        [Authorize]
        [HttpPost("change-password")]
        public IActionResult ChangePassword(string password)
        {
            var claims = GetUserClaims();

            if (password != null)
            {
                var user = _context.User.Where(e => e.Email == claims.Email).FirstOrDefault();
                
                if (user != null)
                {
                    user.Password = password;
                    _context.SaveChanges();
                    var token = AuthHandler.GenerateToken(user);
                    return Ok(token);
                }
                return Unauthorized("Token Expired");
            }
            return BadRequest("Invalid Password");
        }

        private DecodedJwt GetUserClaims()
        {
            var access = new HttpContextAccessor();
            string authHeader = access.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                string jwtToken = authHeader.Substring("Bearer ".Length).Trim();
                return DecodeJwt(jwtToken);
            }

            return null;
        }

        private static DecodedJwt DecodeJwt(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedjwt = tokenHandler.ReadJwtToken(jwtToken);

            return new DecodedJwt
            {
                Email = decodedjwt.Claims.First(claim => claim.Type == "email").Value,
                Username = decodedjwt.Claims.First(claim => claim.Type == "nome").Value
            };
        }
    }
}
