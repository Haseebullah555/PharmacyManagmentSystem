using Application.Dtos.Common;

namespace Application.Dtos.InventoryStock
{
    public class UpdateInventoryStockDto : UpdateBaseDto
    {
        public int InventoryBatchID { get; set; }
        public int LocationID { get; set; }
        public int MedicineUnitID { get; set; }
        public decimal Quantity { get; set; }
    }
}
