namespace Application.Dtos.SaleItem
{
     public class UpdateSaleItemDto
    {
        // Existing item = ID has value
        // New item = ID is null
        public int? Id { get; set; }

        public int MedicineId { get; set; }

        public int MedicineUnitId { get; set; }

        public int InventoryBatchId { get; set; }

        public int LocationId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }
    }
}