using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineID { get; set; }
        public string? TradeName { get; set; }
        public string? GenericName { get; set; }
        public string? Capacity { get; set; }

    }
}
