using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Purchase : BaseDomainEntity
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public Decimal UnitPrice { get; set; }
        public Decimal SalePrice { get; set; }
        public Decimal TotalPrice { get; set; }
        [Required]
        public Decimal Paid { get; set; }
        [Required]
        public Decimal Unpaid { get; set; }
        [Required]
        public DateOnly PurchaseDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string BatchNumber { get; set; }
        public string InvoiceNumber { get; set; }
        //Navigation Property
        public Medicine Medicine { get; set; }
        public int MedicineID { get; set; }
        public Supplier Supplier { get; set; }
        public int SupplierID { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyID { get; set; }
        public int? InventoryBatchID { get; set; }
        public InventoryBatch InventoryBatch { get; set; }


    }
}
