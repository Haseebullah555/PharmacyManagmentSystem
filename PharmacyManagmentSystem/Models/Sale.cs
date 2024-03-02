using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleID { get; set; }
        public Customer CustomerID { get; set; }
        public Medicine MedicineID { get; set; }
        public Catagory CatagoryID { get; set; }
        public Company CompanyID { get; set; }
        public Currency CurrencyID { get; set; }
        [Required(ErrorMessage = "Amount Couldn't be Empty")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Unite Price Couldn't be Empty")]
        public int UnitPrice { get; set; }
        public int SubTotalPrice { get; set; }
        public int TotalPrice { get; set; }
        [Required(ErrorMessage = "Paid Amount Couldn't be Empty")]
        public int Paid { get; set; }
        [Required(ErrorMessage = "Unpaid Amount Couldn't be Empty")]
        public int Unpaid { get; set; }
        [Required(ErrorMessage = "Sale Date Couldn't be Empty")]
        [DataType(DataType.DateTime)]
        public DateTime SaleDate { get; set; }
    }
}
