namespace Application.Dtos.PurchaseItem
{
    public class PurchaseItemDto
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int MedicineId { get; set; }
        public int MedicineUnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int InventoryBatchId { get; set; }
    }
}
