using Microsoft.AspNetCore.Identity;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAdminUserAsync(IServiceProvider services, IConfiguration config)
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        var adminEmail = config["AdminSeed:Email"]!;
        var adminPassword = config["AdminSeed:Password"]!;

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin is null)
        {
            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Admin User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to seed admin user: {errors}");
            }
        }
    }
}