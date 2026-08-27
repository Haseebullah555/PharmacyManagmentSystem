using Application.Contracts.UserManagement;
using Domain.Models.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

public class UserSeeder : IUserSeeder
{
    private readonly AppDbContext _context;

    public UserSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Check if admin already exists
        if (await _context.Users.AnyAsync(u => u.UserName == "admin"))
            return;

        // Get Admin role
        var adminRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "Admin");

        if (adminRole == null)
            throw new Exception("Admin role not found. Seed roles first.");

        // Create admin user
        var adminUser = new User
        {
            Id = Guid.Parse("b2f8e888-dfc9-4c4a-9b76-d7f0a83f5101"),
            UserName = "admin",
            Email = "admin@gmail.com",
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryTime = DateTime.UtcNow.AddYears(5)
        };

        // Hash password properly
        var passwordHasher = new PasswordHasher<User>();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

        // Assign Admin role
        adminUser.UserRoles.Add(new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();
    }
}
