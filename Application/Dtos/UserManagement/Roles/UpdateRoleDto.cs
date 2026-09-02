using Application.Dtos.Common;

namespace Application.Dtos.UserManagement.Roles
{
    public class UpdateRoleDto : UpdateBaseDto
    {
        public string Name { get; set; } = string.Empty;
    }
}