using Application.Dtos.Common;
using Application.Dtos.SaleItem;

namespace Application.Dtos.Sale
{
    public class UpdateSaleDto : UpdateBaseDto
    {
        public int Id { get; set; }

        public DateOnly SaleDate { get; set; }

        public int? CustomerId { get; set; }

        public int CurrencyId { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal Discount { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? Remarks { get; set; }

        public List<UpdateSaleItemDto> Items { get; set; } = new();
    }
}
