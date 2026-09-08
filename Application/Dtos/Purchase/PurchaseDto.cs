using Application.Dtos.PurchaseItem;

namespace Application.Dtos.Purchase
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int SupplierId { get; set; }
        public string Supplier { get; set; }
        
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public string Remarks { get; set; }
        public List<PurchaseItemDto> Items { get; set; }
            = new List<PurchaseItemDto>();
    }
}
