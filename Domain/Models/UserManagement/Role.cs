using Domain.Common;

namespace Domain.Models.UserManagement
{
    public class Role : UserManagenetBaseDomainEntity
    {
        public string Name { get; set; } = string.Empty; // Admin, Manager
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}