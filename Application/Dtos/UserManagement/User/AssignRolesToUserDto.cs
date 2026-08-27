namespace Application.Dtos.UserManagement.User
{
    public class AssignRolesToUserDto
    {
        public Guid UserId { get; set; }        
        public List<Guid> RoleIds { get; set; } = new();
    }
}