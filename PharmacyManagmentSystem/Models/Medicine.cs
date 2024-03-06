using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Medicine
    {
        public int MedicineID { get; set; }
        [Required(ErrorMessage = "Generic Name Couldn't be Empty")]
        public string GenericName { get; set; }
        [Required(ErrorMessage ="Trade Name Couldn't be Empty")]
        public string TradeName { get; set; }

        [Required(ErrorMessage = "Capacity Couldn't be Empty")]
        public string Capacity { get; set; }
        // Navigation Property
        public Category Category{ get; set; }
        public int CategoryID { get; set; }
        public Company Company { get; set; }
        public int CompanyID { get; set; }
        public ICollection<Sale> Sales { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }

    }
}
