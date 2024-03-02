using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Purchases
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Purchases PurchaseID { get; set; }
        public Suppliers SupplierID { get; set; }
        public Medicines MedicineID { get; set; }
        public Catagories CatagoryID { get; set; }
        public Companies CompanyID { get; set; }
        public Currencies CurrencyID { get; set; }
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
        [Required(ErrorMessage = "Purchase Date Couldn't be Empty")]
        [DataType(DataType.DateTime)]
        public DateTime PurchaseDate { get; set; }

    }
}
