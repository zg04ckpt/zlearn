using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SeedData
    {
        private const string ADMIN_EMAIL = "ADMIN_EMAIL";
        private const string ADMIN_PASSWORD = "ADMIN_PASSWORD";

        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<AppUser> userManager)
        {
            ILogger<SeedData> logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();

            logger.LogInformation("Seeding data...");

            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            string[] roleNames = { "Admin", "Editor" };
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

            //temp
            var editorEmail = "lapyen30@gmail.com";
            var editorPassword = "khoihv.123.YLPT";

            if (adminEmail == null || adminPassword == null)
            {
                
                logger.LogError("Admin email and password must be configured in environment variables.");
                throw new Exception("Admin email and password must be configured in environment variables.");
            }

            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Nguyên",
                LastName = "Hoàng"
            };

            var editor = new AppUser
            {
                UserName = editorEmail,
                Email = editorEmail,
                FirstName = "Hoàng",
                LastName = "Khởi"
            };

            var user = await userManager.FindByEmailAsync(adminEmail);
            if (user == null)
            {
                var createdAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createdAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    var errors = string.Join(",", createdAdmin.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
            }

            var editorUser = await userManager.FindByEmailAsync(editorEmail);
            if (editorUser == null)
            {
                var createdEditor = await userManager.CreateAsync(editor, editorPassword);
                if (createdEditor.Succeeded)
                {
                    await userManager.AddToRoleAsync(editor, "Editor");
                }
                else
                {
                    var errors = string.Join(",", createdEditor.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
            }
        }
    }
}
