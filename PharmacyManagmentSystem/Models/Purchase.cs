using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }

        [Required(ErrorMessage = "Amount Couldn't be Empty")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Unite Price Couldn't be Empty")]
        public Decimal UnitPrice { get; set; }
        public Decimal SalePrice { get; set; }
        public Decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Paid Amount Couldn't be Empty")]
        public Decimal Paid { get; set; }
        [Required(ErrorMessage = "Unpaid Amount Couldn't be Empty")]
        public Decimal Unpaid { get; set; }
        [Required(ErrorMessage = "Purchase Date Couldn't be Empty")]
        public DateTime PurchaseDate { get; set; }
        //Navigation Property
        public Medicine Medicine { get; set; }
        public int MedicineID { get; set; }
        public Supplier Supplier { get; set; }
        public int SupplierID { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyID { get; set; }


    }
}
