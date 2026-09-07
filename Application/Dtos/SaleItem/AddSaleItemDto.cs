namespace Application.Dtos.SaleItem
{
    public class AddSaleItemDto
    {
        public int MedicineId { get; set; }

        public int MedicineUnitId { get; set; }

        public int InventoryBatchId { get; set; }

        public int LocationID { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }
    }
}