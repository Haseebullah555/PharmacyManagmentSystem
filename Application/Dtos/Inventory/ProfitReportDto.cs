namespace Application.Dtos.Inventory
{
    public class ProfitReportDto
    {
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal QuantitySold { get; set; }
    }
}