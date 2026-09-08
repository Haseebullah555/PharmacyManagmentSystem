using Application.Dtos.Common;
using Application.Dtos.PurchaseItem;

namespace Application.Dtos.Purchase
{
    public class AddPurchaseDto : CreateBaseDto
    {
        public DateOnly PurchaseDate { get; set; }

        public string? InvoiceNumber { get; set; }

        public int SupplierId { get; set; }

        public int CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }

        public decimal PaidAmount { get; set; }

        public string? Remarks { get; set; }

        public List<AddPurchaseItemDto> Items { get; set; }
            = new List<AddPurchaseItemDto>();
    }
}
