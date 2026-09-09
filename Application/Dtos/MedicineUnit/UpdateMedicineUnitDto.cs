using Application.Dtos.Common;

namespace Application.Dtos.MedicineUnit
{
    public class UpdateMedicineUnitDto : UpdateBaseDto
    {
        public int MedicineId { get; set; }
        public int UnitId { get; set; }
        public decimal ConversionFactor { get; set; }
        public bool IsBaseUnit { get; set; }
    }
}
