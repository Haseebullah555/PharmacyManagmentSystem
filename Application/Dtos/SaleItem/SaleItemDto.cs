namespace Application.Dtos.SaleItem
{
    public class SaleItemDto
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }
        public string? MedicineName { get; set; }

        public int MedicineUnitId { get; set; }
        public string? MedicineUnitName { get; set; }

        public int InventoryBatchId { get; set; }
        public string? BatchNumber { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        public int LocationId { get; set; }
        public string? LocationName { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}