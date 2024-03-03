using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleID { get; set; }
        

        [Required(ErrorMessage = "Amount Couldn't be Empty")]
        public Decimal Amount { get; set; }
        [Required(ErrorMessage = "Unite Price Couldn't be Empty")]
        public Decimal UnitPrice { get; set; }
        public Decimal SubTotalPrice { get; set; }
        public Decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Paid Amount Couldn't be Empty")]
        public Decimal Paid { get; set; }
        [Required(ErrorMessage = "Unpaid Amount Couldn't be Empty")]
        public Decimal Unpaid { get; set; }
        [Required(ErrorMessage = "Sale Date Couldn't be Empty")]
        [DataType(DataType.DateTime)]
        public DateTime SaleDate { get; set; }
        //Navigation Property
        public Medicine Medicine { get; set; }
        public int MedicineID { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyID { get; set; }

        public Customer Customer { get; set; }
        public int CustomerID { get; set; }
    }
}
