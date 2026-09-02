using Application.Dtos.Common;

namespace Application.Dtos.Dosage
{
    public class AddDosageDto : CreateBaseDto
    {
        public string DosageName { get; set; }
        public int CategoryId { get; set; }
    }
}