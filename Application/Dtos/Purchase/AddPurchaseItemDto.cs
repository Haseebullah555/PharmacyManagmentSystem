using Application.Dtos.Common;

namespace Application.Dtos.Purchase
{
    public class AddPurchaseItemDto : CreateBaseDto
    {
        public int PurchaseID { get; set; }
        public int MedicineID { get; set; }
        public int MedicineUnitID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int InventoryBatchID { get; set; }
    }
}
