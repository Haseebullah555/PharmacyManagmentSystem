namespace Application.Dtos.UserManagement.Roles
{
    public class RolePermissionDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
