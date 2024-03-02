using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Medicines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicineID { get; set; }
        [Required(ErrorMessage ="Trade Name Couldn't be Empty")]
        public string? TradeName { get; set; }
        [Required(ErrorMessage = "Generic Name Couldn't be Empty")]
        public string? GenericName { get; set; }
        [Required(ErrorMessage = "Capacity Couldn't be Empty")]
        public string? Capacity { get; set; }

    }
}
