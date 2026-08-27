using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class SaleReturn : BaseDomainEntity
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string Reason { get; set; }
        public bool Restock { get; set; }
        public DateOnly ReturnDate { get; set; }

        public int SaleBatchAllocationID { get; set; }
        public SaleBatchAllocation SaleBatchAllocation { get; set; }
    }
}