using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }
        private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByNameAsync("admin@bookstore.com") == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@bookstore.com"
                };
                var result = await userManager.CreateAsync(user, "P@sword1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            if (await userManager.FindByNameAsync("customer1@gmail.com") == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "customer1",
                    Email = "customer1@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "P@sword1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
            if (await userManager.FindByNameAsync("customer2@gmail.com") == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "customer2",
                    Email = "customer2@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "P@sword1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
        }
    }
}
