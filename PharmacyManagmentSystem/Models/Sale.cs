using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.Models
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }
        public int CustomerID { get; set; }
        public int MedicineID { get; set; }
        public int CatagoryID { get; set; }
        public int CompanyID { get; set; }
        public int CurrencyID { get; set; }
        public int Amount { get; set; }
        public int UnitPrice { get; set; }
        public int SubTotalPrice { get; set; }
        public int TotalPrice { get; set; }
        public int Paid { get; set; }
        public int Unpaid { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
