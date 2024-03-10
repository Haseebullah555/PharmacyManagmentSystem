using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Customer Name Couldn't be Empty")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNo { get; set; }
        public  string Address { get; set; }
     
    }
}
