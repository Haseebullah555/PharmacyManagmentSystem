using System.Reflection;
using Application.Contracts.UserManagement;
using Domain.Models.UserManagement;
using Identity.Utils;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories.UserManagement
{
    public class PermissionSeeder : IPermissionSeeder
    {
        private readonly AppDbContext _context;

        public PermissionSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var permissions = typeof(SystemClaims)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => f.GetValue(null)?.ToString())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct();

            foreach (var permissionCode in permissions)
            {
                if (!await _context.Permissions.AnyAsync(p => p.Code == permissionCode))
                {
                    _context.Permissions.Add(new Permission
                    {
                        Code = permissionCode!,
                        Name = permissionCode!.Replace("_", " ")
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}