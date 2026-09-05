namespace Application.Dtos.SaleItem
{
    public class AddSaleItemDto
    {
        public int MedicineID { get; set; }

        public int MedicineUnitID { get; set; }

        public int InventoryBatchID { get; set; }

        public int LocationID { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }
    }
}