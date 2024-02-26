using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseID { get; set; }
        public int SupplierID { get; set; }
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
        public DateTime PurchaseDate { get; set; }

    }
}
