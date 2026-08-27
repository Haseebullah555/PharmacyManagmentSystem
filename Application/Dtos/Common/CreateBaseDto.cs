namespace Application.Dtos.Common
{
    public class CreateBaseDto
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}