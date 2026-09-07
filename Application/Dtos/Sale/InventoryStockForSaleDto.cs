namespace Application.Dtos.Sale
{
    public class InventoryStockForSaleDto
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }
        public string MedicineName { get; set; }

        public int InventoryBatchId { get; set; }
        public string BatchNumber { get; set; }

        public int MedicineUnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitShortName { get; set; }

        public int LocationID { get; set; }
        public string LocationName { get; set; }

        public decimal Quantity { get; set; }
        public DateOnly? ExpiryDate { get; set; }
    }
}