using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class InventoryStock : BaseDomainEntity
    {
        [Required]
        public int InventoryBatchId { get; set; }
        public InventoryBatch InventoryBatch { get; set; }

        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Required]
        public int MedicineUnitId { get; set; }
        public MedicineUnit MedicineUnit { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}