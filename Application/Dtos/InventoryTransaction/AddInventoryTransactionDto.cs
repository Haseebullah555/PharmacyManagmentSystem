using Application.Dtos.Common;
using Domain.Enums;

namespace Application.Dtos.InventoryTransaction
{
    public class AddInventoryTransactionDto : CreateBaseDto
    {
        public int MedicineId { get; set; }
        public int InventoryBatchId { get; set; }
        public int MedicineUnitId { get; set; }
        public int LocationID { get; set; }
        public decimal Quantity { get; set; }
        public InventoryTransactionType TransactionType { get; set; }
        public InventoryReferenceType ReferenceType { get; set; }
        public int? ReferenceID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Description { get; set; }
    }
}
