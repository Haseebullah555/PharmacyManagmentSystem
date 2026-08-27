using Application.Contracts.UserManagement;
using Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders.UserManagement
{
    public class RolePermissionSeeder : IRolePermissionSeeder
    {
        private readonly AppDbContext _context;

        public RolePermissionSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // ===================== ADMIN ROLE =====================
            var adminRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Admin");

            if (adminRole == null)
                throw new Exception("Admin role not found.");

            // ===================== ALL PERMISSIONS =====================
            var permissions = await _context.Permissions
                .AsNoTracking()
                .ToListAsync();

            if (!permissions.Any())
                return;

            // ===================== EXISTING ROLE PERMISSIONS =====================
            var existingPermissionIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == adminRole.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            // ===================== ADD MISSING PERMISSIONS =====================
            var newRolePermissions = permissions
                .Where(p => !existingPermissionIds.Contains(p.Id))
                .Select(p => new RolePermission
                {
                    RoleId = adminRole.Id,
                    PermissionId = p.Id
                })
                .ToList();

            if (newRolePermissions.Any())
            {
                _context.RolePermissions.AddRange(newRolePermissions);
                await _context.SaveChangesAsync();
            }
        }
    }

}