namespace Application.Dtos.Sale
{
    public class SaleDto
    {
        public int Id { get; set; }
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
        public string Medicine { get; set; }
        public int CurrencyID { get; set; }
        public string Currency { get; set; }
        public int CustomerID { get; set; }
        public string Customer { get; set; }
        public int? InventoryBatchID { get; set; }
    }
}
