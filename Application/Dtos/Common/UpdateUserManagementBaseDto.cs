namespace Application.Dtos.Common
{
    public class UpdateUserManagementBaseDto
    {
        public Guid Id { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}