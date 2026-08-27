namespace Application.Dtos.Common
{
    public class UpdateBaseDto
    {
        public int Id { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}