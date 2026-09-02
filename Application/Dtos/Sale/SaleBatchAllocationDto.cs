namespace Application.Dtos.Sale
{
    public class SaleBatchAllocationDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int SaleID { get; set; }
        public int InventoryBatchID { get; set; }
    }
}
