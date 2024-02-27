using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Customer Name Couldn't be Empty")]
        public string? CustomerName { get; set; }
        public required string location { get; set; }

    }
}
