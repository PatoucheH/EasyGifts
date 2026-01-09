using EasyGiftsBackend.Domain.Entities;
using EasyGiftsBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

public static class AdminSeed
{
    public static async Task SeedAdminAsync(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext context)
    {
        var adminEmail = "admin@easygifts.com";
        var adminPassword = "Admin123!";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new IdentityUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, adminPassword);
            await userManager.AddToRoleAsync(admin, "Admin");

            context.AppUsers.Add(new User
            {
                IdentityId = admin.Id,
                Username = "admin",
                Email = adminEmail
            });

            await context.SaveChangesAsync();
        }
    }
}
