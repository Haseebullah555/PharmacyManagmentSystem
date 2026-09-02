using Application.Dtos.Common;

namespace Application.Dtos.Dosage
{
    public class UpdateDosageDto : UpdateBaseDto
    {
        public string DosageName { get; set; }
        public int CategoryId { get; set; }
    }
}