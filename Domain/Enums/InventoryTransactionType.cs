namespace Domain.Enums
{
    public enum InventoryTransactionType
    {
        Purchase = 1,
        Sale = 2,
        SaleReturn = 3,
        PurchaseReturn = 4,
        AdjustmentIn = 5,
        AdjustmentOut = 6,
        TransferIn = 7,
        TransferOut = 8,
        Damaged = 9,
        Expired = 10
    }
}