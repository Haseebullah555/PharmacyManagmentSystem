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
        public string ContactNo { get; set; }
        public string Description { get; set; }
    }
}
