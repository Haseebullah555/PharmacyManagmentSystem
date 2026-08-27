using Application.Dtos.Common;

namespace Application.Dtos.UserManagement.Roles
{
    public class AddRoleDto : CreateBaseDto
    {
        public string Name { get; set; } = string.Empty;

    }
}