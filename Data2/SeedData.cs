using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Data
{
    public class SeedData
    {
        private const string ADMIN_EMAIL = "ADMIN_EMAIL";
        private const string ADMIN_PASSWORD = "ADMIN_PASSWORD";

        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<AppUser> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            string[] roleNames = { Consts.DEFAULT_ADMIN_ROLE, Consts.DEFAULT_USER_ROLE };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    AppRole role = new AppRole
                    {
                        Name = roleName,
                        Description = "Created by default"
                    };
                    await roleManager.CreateAsync(role);
                }
            }

            var adminEmail = Environment.GetEnvironmentVariable(ADMIN_EMAIL);
            var adminPassword = Environment.GetEnvironmentVariable(ADMIN_PASSWORD);

            if (adminEmail == null || adminPassword == null)
            {
                throw new Exception("Admin email and password must be configured in environment variables.");
            }

            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Nguyên",
                LastName = "Hoàng"
            };

            var user = await userManager.FindByEmailAsync(adminEmail);
            if (user == null)
            {
                var createdAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createdAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Consts.DEFAULT_ADMIN_ROLE);
                }
                else
                {
                    var errors = string.Join(",", createdAdmin.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
            }
        }
    }
}
