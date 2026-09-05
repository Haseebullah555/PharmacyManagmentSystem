using Application.Dtos.Common;
using Application.Dtos.SaleItem;

namespace Application.Dtos.Sale
{
    public class AddSaleDto : CreateBaseDto
    {
        public DateOnly SaleDate { get; set; }

        public int? CustomerID { get; set; }

        public int CurrencyID { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal Discount { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? Remarks { get; set; }

        public List<AddSaleItemDto> Items { get; set; } = new();
    }
}
