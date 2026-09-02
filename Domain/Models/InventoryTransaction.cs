using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.Enums;

namespace Domain.Models
{
    public class InventoryTransaction : BaseDomainEntity
    {
        [Required]
        public int MedicineID { get; set; }
        public Medicine Medicine { get; set; }

        [Required]
        public int InventoryBatchID { get; set; }
        public InventoryBatch InventoryBatch { get; set; }

        [Required]
        public int MedicineUnitID { get; set; }
        public MedicineUnit MedicineUnit { get; set; }

        [Required]
        public int LocationID { get; set; }
        public Location Location { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public InventoryTransactionType TransactionType { get; set; }

        [Required]
        public InventoryReferenceType ReferenceType { get; set; }

        public int? ReferenceID { get; set; }

        public DateTime TransactionDate { get; set; }

        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}