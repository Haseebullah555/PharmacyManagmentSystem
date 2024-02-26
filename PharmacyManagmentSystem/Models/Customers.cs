using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.Models
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public required string location { get; set; }

    }
}
