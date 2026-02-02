using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Domain.Entities;

namespace Portfolio.Infrastructure;

public static class AdminSeed
{
    public static async Task SeedAsync(IServiceProvider service)
    {
        var userManager = service.GetRequiredService<UserManager<User>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        const string roleName = "Admin";
        const string email = "hebibovsamir26@gmail.com";
        const string password = "hebibovS13!";

        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FullName = "Samir Həbibov",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}
