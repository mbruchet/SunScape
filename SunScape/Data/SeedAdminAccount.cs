using Microsoft.AspNetCore.Identity;

namespace SunScape.Data;

public class SeedAdminAccount
{
    public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
    {
        var username = configuration["AdminUser:Username"];
        var email = configuration["AdminUser:Email"];
        var password = configuration["AdminUser:Password"];
        var adminRoleName = configuration["AdminUser:Role"];

        if(!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = adminRoleName });
        }

        if (await userManager.FindByNameAsync(username) == null)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Optionally assign roles if needed
                await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }
    }
}
