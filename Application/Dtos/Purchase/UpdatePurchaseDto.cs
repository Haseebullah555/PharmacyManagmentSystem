using Application.Dtos.Common;

namespace Application.Dtos.Purchase
{
    public class UpdatePurchaseDto : UpdateBaseDto
    {
        public DateOnly PurchaseDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public int SupplierID { get; set; }
        public int CurrencyID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public string? Remarks { get; set; }
    }
}
