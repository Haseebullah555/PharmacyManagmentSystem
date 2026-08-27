using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class SaleBatchAllocation : BaseDomainEntity
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public int SaleID { get; set; }
        public Sale Sale { get; set; }
        public int InventoryBatchID { get; set; }
        public InventoryBatch InventoryBatch { get; set; }
        public ICollection<SaleReturn> SaleReturns { get; set; } = new List<SaleReturn>();
    }
}