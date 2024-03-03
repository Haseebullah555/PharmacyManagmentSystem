using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }
        [Required(ErrorMessage = "Supplier Name Couldn't be Empty")]
        public string SupplierName { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Supplier Phone Number Couldn't be Empty")]
        [RegularExpression("^937[0-9]{0,11}$|7[0-9]{0,9}$", ErrorMessage ="Invalid Phone Number")]
        public string ContactNo { get; set; }
        public string Description { get; set; }
        //Navigation Property
        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<Medicine> Medicines { get; set; }
    }
}
