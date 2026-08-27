using Application.Contracts.UserManagement;
using Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders.UserManagement
{
    public class RoleSeeder : IRoleSeeder
    {
        private readonly AppDbContext _context;

        public RoleSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var roles = new List<Role>
            {
                new Role
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Admin"
                },
                new Role
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Manager"
                },
                new Role
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "User"
                }
            };

            foreach (var role in roles)
            {
                if (!await _context.Roles.AnyAsync(r => r.Name == role.Name))
                {
                    _context.Roles.Add(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}