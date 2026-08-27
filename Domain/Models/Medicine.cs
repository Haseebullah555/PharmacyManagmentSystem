using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Medicine : BaseDomainEntity
    {
        [Required]
        public string GenericName { get; set; }
        [Required]
        public string TradeName { get; set; }

        [Required]
        public string Capacity { get; set; }
        [Required]
        public string UnitOfMeasure { get; set; }
        public string Barcode { get; set; }
        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; }
        public bool IsActive { get; set; } = true;
        public bool RequiresPrescription { get; set; }
        // Navigation
        [Required]
        public int CategoryID { get; set; }
        public Category Category{ get; set; }
        [Required]
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();
        public ICollection<InventoryAdjustment> InventoryAdjustments { get; set; } = new List<InventoryAdjustment>();
         
    }
}
