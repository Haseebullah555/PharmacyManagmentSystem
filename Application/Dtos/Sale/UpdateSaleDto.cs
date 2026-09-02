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
        public int MedicineID { get; set; }
        public int CurrencyID { get; set; }
        public int CustomerID { get; set; }
        public int? InventoryBatchID { get; set; }
    }
}
