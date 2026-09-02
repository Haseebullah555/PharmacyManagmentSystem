using Application.Dtos.Common;

namespace Application.Dtos.UserManagement.User
{
    public class UpdateUserRoleDto : UpdateUserManagementBaseDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
