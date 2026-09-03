using Application.Dtos.Common;
using Application.Dtos.PurchaseItem;

namespace Application.Dtos.Purchase
{
    public class AddPurchaseDto : CreateBaseDto
    {
        public DateOnly PurchaseDate { get; set; }

        public string? InvoiceNumber { get; set; }

        public int SupplierID { get; set; }

        public int CurrencyID { get; set; }

        public decimal PaidAmount { get; set; }

        public string? Remarks { get; set; }

        public List<AddPurchaseItemDto> Items { get; set; }
            = new List<AddPurchaseItemDto>();
    }
}
