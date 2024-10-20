
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class SeedData
    {
        private const string ADMIN_EMAIL = "ADMIN_EMAIL";
        private const string ADMIN_PASSWORD = "ADMIN_PASSWORD";

        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<AppUser> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            string[] roleNames = { "Admin", "User" };
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

            var user = await userManager.FindByEmailAsync(adminEmail);
            if (user == null)
            {
                var admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Nguyên",
                    LastName = "Hoàng",
                    CreatedDate = DateOnly.FromDateTime(DateTime.Today).ToString(),
                    EmailConfirmed = true,
                };

                var createdAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createdAdmin.Succeeded)
                {
                    var result = await userManager.AddToRolesAsync(admin, roleNames);
                    if(!result.Succeeded)
                    {
                        return;
                    }
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
