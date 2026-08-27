namespace Application.Dtos.UserManagement.Roles
{
    public class RolesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> RolePermissions { get; set; } = new();
    }
}