namespace Application.Dtos.MedicineUnit
{
    public class MedicineUnitDto
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string Medicine { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public decimal ConversionFactor { get; set; }
        public bool IsBaseUnit { get; set; }
    }
}
