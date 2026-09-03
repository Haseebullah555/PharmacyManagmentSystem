using Application.Dtos.Common;

namespace Application.Dtos.PurchaseItem
{
    public class AddPurchaseItemDto
    {
        public int MedicineID { get; set; }

        public int MedicineUnitID { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string BatchNumber { get; set; }

        public DateOnly? ManufacturingDate { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public int LocationID { get; set; }
    }
}
