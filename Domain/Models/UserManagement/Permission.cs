namespace Domain.Models.UserManagement
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;   // e.g. CreateProject
        public string Code { get; set; } = string.Empty;   // e.g. PROJECT_CREATE
    }
}