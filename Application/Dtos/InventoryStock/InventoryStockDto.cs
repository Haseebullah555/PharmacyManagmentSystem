namespace Application.Dtos.InventoryStock
{
    public class InventoryStockDto
    {
        public int Id { get; set; }
        public int InventoryBatchId { get; set; }
        public int LocationID { get; set; }
        public int MedicineUnitId { get; set; }
        public decimal Quantity { get; set; }
    }
}
