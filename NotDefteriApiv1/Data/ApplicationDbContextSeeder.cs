using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotDefteriApiv1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotDefteriApiv1.Data
{
    public static class ApplicationDbContextSeeder
    {
        public async static Task SeedUserAndRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (!await userManager.Users.AnyAsync(x=>x.UserName == "admin@example.com"))
            {
                var user = new AppUser()
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(user, "Password1.");
                await userManager.AddToRoleAsync(user, "admin");
            }
        }

        public static async Task<IHost> SeedAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var env = serviceProvider.GetRequiredService<IHostEnvironment>();
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await ApplicationDbContextSeeder.SeedUserAndRolesAsync(roleManager, userManager);
            }
            return host;
        }
    }
}
