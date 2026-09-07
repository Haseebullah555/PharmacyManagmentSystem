using Application.Dtos.Common;

namespace Application.Dtos.Purchase
{
    public class UpdatePurchaseDto : UpdateBaseDto
    {
        public DateOnly PurchaseDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public int SupplierId { get; set; }
        public int CurrencyId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public string? Remarks { get; set; }
    }
}
