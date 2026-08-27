namespace Application.Dtos.Inventory
{
    public class DailySalesDto
    {
        public DateOnly Date { get; set; }
        public int TransactionCount { get; set; }
        public decimal QuantitySold { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalUnpaid { get; set; }
    }
}