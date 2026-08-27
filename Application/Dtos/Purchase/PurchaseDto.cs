namespace Application.Dtos.Purchase
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Paid { get; set; }
        public decimal Unpaid { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int MedicineID { get; set; }
        public int SupplierID { get; set; }
        public int CurrencyID { get; set; }
    }
}
