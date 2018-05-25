using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Dictionary.Data;
using Dictionary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Dictionary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DictionaryDbContext _dictionaryDbContext;
        public AccountController(IConfiguration configuration, DictionaryDbContext dictionaryDbContext)
        {
            _configuration = configuration;
            _dictionaryDbContext = dictionaryDbContext;
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return _dictionaryDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public ActionResult Register([FromBody]User user)
        {
            var selectedUser = _dictionaryDbContext.Users.FirstOrDefault(x => x.Email == user.Email);
            if (selectedUser != null)
            {
                return BadRequest();
            }

            user.Password = HashPassword(user.Password);
            user.IsActive = true;
            user.CreatedOn = DateTime.UtcNow;
            _dictionaryDbContext.Users.Add(user);
            var created = _dictionaryDbContext.SaveChanges() == 1;
            if (created)
            {
                return CreatedAtAction("Get", new { id = user.Id }, new { Email = user.Email, Id = user.Id, CreatedOn = DateTime.UtcNow });
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public ActionResult Post([FromBody]User user)
        {
            var selectedUser = _dictionaryDbContext.Users.FirstOrDefault(x => x.Email == user.Email
                && x.Password == HashPassword(user.Password));
            if (selectedUser == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, selectedUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, selectedUser.Id.ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Security:Issuer"],
                audience: _configuration["Security:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:SigningKey"])),
                        SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = DateTime.UtcNow.AddDays(60)
            });
        }

        private string HashPassword(string password)
        {
            byte[] salt = Encoding.Unicode.GetBytes(_configuration.GetValue<string>("Security:Salt"));
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}