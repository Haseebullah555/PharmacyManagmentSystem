namespace Application.Dtos.InventoryStock
{
    public class InventoryStockDto
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }
        public string MedicineName { get; set; }

        public int InventoryBatchId { get; set; }
        public string BatchNumber { get; set; }

        public DateOnly? ManufacturingDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        public int MedicineUnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitShortName { get; set; }

        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public decimal Quantity { get; set; }
    }
}
