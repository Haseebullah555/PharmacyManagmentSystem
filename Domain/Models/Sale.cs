using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Sale : BaseDomainEntity
    {
        [Required]
        public Decimal SaleAmount { get; set; }
        [Required]
        public Decimal UnitPrice { get; set; }
        public Decimal TotalPrice { get; set; }
        [Required]
        public Decimal Paid { get; set; }
        [Required]
        public Decimal Unpaid { get; set; }
        [Required]
        public DateOnly SaleDate { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; }
        //Navigation Property
        public Medicine Medicine { get; set; }
        public int MedicineID { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyID { get; set; }

        public Customer Customer { get; set; }
        public int CustomerID { get; set; }
        public int? InventoryBatchID { get; set; }
        public InventoryBatch InventoryBatch { get; set; }
        public ICollection<SaleBatchAllocation> BatchAllocations { get; set; } = new List<SaleBatchAllocation>();
    }
}
