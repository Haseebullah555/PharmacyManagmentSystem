using Application.Dtos.Common;

namespace Application.Dtos.UserManagement.Roles
{
    public class UpdateRolePermissionDto : UpdateUserManagementBaseDto
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
