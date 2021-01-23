using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotDefteriApiv1.Dtos;
using NotDefteriApiv1.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotDefteriApiv1
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext db;

        // GET: api/<LoginController>
        public LoginController(IConfiguration configuration, SignInManager<AppUser> signInManager, ApplicationDbContext db)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.db = db;
        }

        // GET api/<LoginController>/5
        [HttpGet]
        public IActionResult Get(string token)
        {
            if (token == null)
            {
                return BadRequest();
            }
            AppUser appUser = db.AppUsers.FirstOrDefault(x => x.Token == token);
            if (appUser != null)
            {
                return Ok(appUser.Email);
            }
            return BadRequest();

        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] LoginDto dto)
        {
            var giris = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
            if (giris.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configuration["Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email,dto.Email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                dto.Token = tokenHandler.WriteToken(token);
                AppUser appUser = db.AppUsers.FirstOrDefault(x => x.Email == dto.Email);
                appUser.Token = dto.Token;
                db.SaveChanges();
                return Ok(dto);
            }
            else
            {
                return BadRequest();
            }
        }


        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
