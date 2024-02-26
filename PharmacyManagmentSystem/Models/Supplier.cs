using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }
        [Required]
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Description { get; set; }
    }
}
