using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CatagoryID { get; set; }
        [Required(ErrorMessage = "Catagory Name Couldn't be Empty")]
        public string CatagoryName { get; set; }
        public Medicine Medicine { get; set; }
        public int MedicineID { get; set; }
    }
}
