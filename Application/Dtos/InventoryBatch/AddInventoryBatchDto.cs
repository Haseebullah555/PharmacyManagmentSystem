using Application.Dtos.Common;

namespace Application.Dtos.InventoryBatch
{
    public class AddInventoryBatchDto : CreateBaseDto
    {
        public int MedicineID { get; set; }
        public string BatchNumber { get; set; }
        public DateOnly? ManufacturingDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
