using Domain.Common;

namespace Domain.Models.UserManagement
{
    public class RolePermission: UserManagenetBaseDomainEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}