namespace Application.Dtos.Purchase
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int SupplierID { get; set; }
        public string Supplier { get; set; }
        
        public int CurrencyID { get; set; }
        public string Currency { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public string Remarks { get; set; }
    }
}
