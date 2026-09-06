namespace Application.Dtos.Sale
{
    public class SaleDto
    {
        public int Id { get; set; }
        public DateOnly SaleDate { get; set; }
        public int? CustomerID { get; set; }
        public string Customer { get; set; }
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public decimal Discount { get; set; }
        public string InvoiceNumber { get; set; }
        public string Remarks { get; set; }

    }
}
