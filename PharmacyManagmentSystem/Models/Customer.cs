using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Customer Name Couldn't be Empty")]
        public string? CustomerName { get; set; }
        [Required]
        [RegularExpression("^937[0-9]{0,11}$|7[0-9]{0,9}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNo { get; set; }
        public  string Address { get; set; }
     
    }
}
