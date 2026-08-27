using Application.Dtos.Common;

namespace Application.Dtos.Medicine
{
    public class UpdateMedicineDto : UpdateBaseDto
    {
        public string GenericName { get; set; }
        public string TradeName { get; set; }
        public string Capacity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Barcode { get; set; }
        public int ReorderLevel { get; set; }
        public bool IsActive { get; set; }
        public bool RequiresPrescription { get; set; }
        public int CategoryID { get; set; }
        public int CompanyID { get; set; }
    }
}
