namespace Application.Dtos.Inventory
{
    public class StockValuationDto
    {
        public int MedicineID { get; set; }
        public string GenericName { get; set; }
        public string TradeName { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal CostValue { get; set; }
        public decimal RetailValue { get; set; }
    }
}