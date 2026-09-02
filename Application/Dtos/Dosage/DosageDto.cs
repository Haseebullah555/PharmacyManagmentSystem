using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Dosage
{
    public class DosageDto
    {
        public int Id { get; set; }
        public string DosageName { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
    }
}