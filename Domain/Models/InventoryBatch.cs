using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class InventoryBatch : BaseDomainEntity
    {
        [Required]
        public int MedicineID { get; set; }
        public Medicine Medicine { get; set; }

        [Required]
        [MaxLength(100)]
        public string BatchNumber { get; set; }

        public DateOnly? ManufacturingDate { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<InventoryStock> Stocks { get; set; }
            = new List<InventoryStock>();
    }
}