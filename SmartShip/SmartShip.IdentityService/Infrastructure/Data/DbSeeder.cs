using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartShip.IdentityService.Domain.Entities;

namespace SmartShip.IdentityService.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IdentityDbContext context)
        {
            var adminExists = await context.Users
                .AnyAsync(u => u.Role == "Admin");

            if (adminExists)
            {
                return;
            }

            var admin = new User
            {
                FullName = "Admin",
                Email = "shubhamy03v@gmail.com",
                Role = "Admin"
            };

            var passwordHasher = new PasswordHasher<User>();

            admin.PasswordHash = passwordHasher.HashPassword(
                admin,
                "Admin123");

            context.Users.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}