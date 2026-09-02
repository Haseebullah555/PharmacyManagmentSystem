using Application.Dtos.Common;

namespace Application.Dtos.UserManagement.User
{
    public class AddUserRoleDto : CreateBaseDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
