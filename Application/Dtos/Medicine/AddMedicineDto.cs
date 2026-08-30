using Application.Dtos.Common;

namespace Application.Dtos.Medicine
{
    public class AddMedicineDto : CreateBaseDto
    {
        public string GenericName { get; set; }
        public string TradeName { get; set; }
        public int DosageId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool RequiresPrescription { get; set; }
        public int CategoryID { get; set; }
        public int CompanyID { get; set; }
    }
}
