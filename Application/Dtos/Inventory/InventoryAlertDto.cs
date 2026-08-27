namespace Application.Dtos.Inventory
{
    public class InventoryAlertDto
    {
        public int MedicineID { get; set; }
        public string GenericName { get; set; }
        public string TradeName { get; set; }
        public string Barcode { get; set; }
        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? NearestExpiryDate { get; set; }
    }
}