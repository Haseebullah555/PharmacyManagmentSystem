using Application.Dtos.Common;

namespace Application.Dtos.Sale
{
    public class UpdateSaleBatchAllocationDto : UpdateBaseDto
    {
        public int Quantity { get; set; }
        public int SaleID { get; set; }
        public int InventoryBatchID { get; set; }
    }
}
