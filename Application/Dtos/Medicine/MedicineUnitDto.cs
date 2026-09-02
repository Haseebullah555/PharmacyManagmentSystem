namespace Application.Dtos.Medicine
{
    public class MedicineUnitDto
    {
        public int Id { get; set; }
        public int MedicineID { get; set; }
        public int UnitID { get; set; }
        public decimal ConversionFactor { get; set; }
        public bool IsBaseUnit { get; set; }
        public bool CanPurchase { get; set; }
        public bool CanSell { get; set; }
    }
}
