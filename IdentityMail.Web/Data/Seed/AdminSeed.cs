using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityMail.Web.Data.Seed
{
    public static class AdminSeed
    {
        public static async Task CreateAdminAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<AppRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<AppUser>>();

            const string adminRoleName = "Admin";
            const string adminEmail = "admin@identitymail.com";
            const string adminUserName = "admin";
            const string adminPassword = "Admin123*";

            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                var roleResult = await roleManager.CreateAsync(
                    new AppRole
                    {
                        Name = adminRoleName
                    });

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(x => x.Description));

                    throw new Exception(
                        $"Admin rolü oluşturulamadı: {errors}");
                }
            }

            var adminUser = await userManager
                .FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    FirstName = "IdentityMail",
                    LastName = "Admin",
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var createResult = await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(x => x.Description));

                    throw new Exception(
                        $"Admin hesabı oluşturulamadı: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(
                    adminUser,
                    adminRoleName))
            {
                var addToRoleResult =
                    await userManager.AddToRoleAsync(
                        adminUser,
                        adminRoleName);

                if (!addToRoleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        addToRoleResult.Errors
                            .Select(x => x.Description));

                    throw new Exception(
                        $"Admin rolü atanamadı: {errors}");
                }
            }
        }
    }
}