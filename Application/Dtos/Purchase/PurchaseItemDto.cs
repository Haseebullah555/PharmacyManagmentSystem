namespace Application.Dtos.Purchase
{
    public class PurchaseItemDto
    {
        public int Id { get; set; }
        public int PurchaseID { get; set; }
        public int MedicineID { get; set; }
        public int MedicineUnitID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int InventoryBatchID { get; set; }
    }
}
