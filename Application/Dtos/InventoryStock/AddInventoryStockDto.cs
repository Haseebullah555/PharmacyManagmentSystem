using Application.Dtos.Common;

namespace Application.Dtos.InventoryStock
{
    public class AddInventoryStockDto : CreateBaseDto
    {
        public int InventoryBatchId { get; set; }
        public int LocationID { get; set; }
        public int MedicineUnitId { get; set; }
        public decimal Quantity { get; set; }
    }
}
