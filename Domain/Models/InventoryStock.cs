using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class InventoryStock : BaseDomainEntity
    {
        [Required]
        public int InventoryBatchID { get; set; }
        public InventoryBatch InventoryBatch { get; set; }

        [Required]
        public int LocationID { get; set; }
        public Location Location { get; set; }

        [Required]
        public int MedicineUnitID { get; set; }
        public MedicineUnit MedicineUnit { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}