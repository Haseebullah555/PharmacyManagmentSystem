using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory
{
    public class InventoryAdjustmentDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateOnly? AdjustmentDate { get; set; }
        [Range(1, int.MaxValue)]
        public int MedicineID { get; set; }
        public int? InventoryBatchID { get; set; }
    }
}