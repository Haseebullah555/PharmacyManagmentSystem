namespace Domain.Models.UserManagement
{
    public class UserManagenetBaseDomainEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}