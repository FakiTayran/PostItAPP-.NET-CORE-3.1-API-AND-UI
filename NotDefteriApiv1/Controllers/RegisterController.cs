using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NotDefteriApiv1.Dtos;
using NotDefteriApiv1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotDefteriApiv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext db;
        private readonly UserManager<AppUser> userManager;


        public RegisterController(IConfiguration configuration, SignInManager<AppUser> signInManager, ApplicationDbContext db, UserManager<AppUser> userManager)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.db = db;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new AppUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true,
                    };
                    if (request.Password != request.ConfirmPassword)
                    {
                        return BadRequest(error: "Şifreler eşleşmiyor");
                    }
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        return Ok("Başarılı");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return BadRequest();
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return BadRequest(error:"Email already exists.");
                }
            }
            return Ok();
        }
    }
}
