using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class InventoryAdjustment : BaseDomainEntity
    {
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateOnly AdjustmentDate { get; set; }

        public int MedicineID { get; set; }
        public Medicine Medicine { get; set; }
        public int? InventoryBatchID { get; set; }
        public InventoryBatch InventoryBatch { get; set; }
    }
}