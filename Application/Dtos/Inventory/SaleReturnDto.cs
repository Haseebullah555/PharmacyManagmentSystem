using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory
{
    public class SaleReturnDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string Reason { get; set; }
        public bool Restock { get; set; } = true;
        public DateOnly ReturnDate { get; set; }
        [Range(1, int.MaxValue)]
        public int SaleBatchAllocationID { get; set; }
    }
}