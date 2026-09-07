using Application.Dtos.Common;

namespace Application.Dtos.Sale
{
    public class UpdateSaleDto : UpdateBaseDto
    {
        public decimal SaleAmount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Paid { get; set; }
        public decimal Unpaid { get; set; }
        public DateOnly SaleDate { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; }
        public int MedicineId { get; set; }
        public int CurrencyId { get; set; }
        public int CustomerId { get; set; }
        public int? InventoryBatchId { get; set; }
    }
}
