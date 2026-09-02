namespace Application.Dtos.Medicine
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string GenericName { get; set; }
        public string TradeName { get; set; }
        public int DosageId { get; set; }
        public string Dosage { get; set; }
        public bool IsActive { get; set; }
        public bool RequiresPrescription { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public int CompanyID { get; set; }
        public string Company { get; set; }
    }
}
