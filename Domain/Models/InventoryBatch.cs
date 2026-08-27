using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class InventoryBatch : BaseDomainEntity
    {
        [Required]
        public string BatchNumber { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        [Range(0, int.MaxValue)]
        public int QuantityReceived { get; set; }
        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }
        public decimal UnitCost { get; set; }
        public decimal SalePrice { get; set; }
        public DateOnly ReceivedDate { get; set; }

        public int MedicineID { get; set; }
        public Medicine Medicine { get; set; }
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }
    }
}