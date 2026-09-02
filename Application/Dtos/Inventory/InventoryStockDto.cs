namespace Application.Dtos.Inventory
{
    public class InventoryStockDto
    {
        public int Id { get; set; }
        public int InventoryBatchID { get; set; }
        public int LocationID { get; set; }
        public int MedicineUnitID { get; set; }
        public decimal Quantity { get; set; }
    }
}
