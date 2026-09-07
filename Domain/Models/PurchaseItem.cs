using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class PurchaseItem : BaseDomainEntity
    {
        [Required]
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        [Required]
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        [Required]
        public int MedicineUnitId { get; set; }
        public MedicineUnit MedicineUnit { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int InventoryBatchId { get; set; }
        public InventoryBatch InventoryBatch { get; set; }
    }
}