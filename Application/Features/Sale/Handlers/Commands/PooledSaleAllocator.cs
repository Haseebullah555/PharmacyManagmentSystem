using System.ComponentModel.DataAnnotations;
using Application.Contracts.Interfaces.Common;
using Application.Dtos.SaleItem;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

internal static class PooledSaleAllocator
{
    internal static async Task AllocateAsync(
        IUnitOfWork unitOfWork,
        Domain.Models.Sale sale,
        AddSaleItemDto item,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var stocks = await unitOfWork.InventoryStocks.Query()
            .Where(stock => stock.Quantity > 0 &&
                stock.InventoryBatch.MedicineId == item.MedicineId &&
                stock.InventoryBatch.IsActive && stock.Location.IsActive)
            .OrderBy(stock => stock.InventoryBatch.ExpiryDate)
            .ThenBy(stock => stock.Id)
            .ToListAsync(cancellationToken);

        var availableQuantity = stocks.Sum(stock => stock.Quantity);
        if (availableQuantity < item.Quantity)
        {
            throw new ValidationException(
                $"Insufficient stock. Available: {availableQuantity}, Requested: {item.Quantity}.");
        }

        var remainingQuantity = item.Quantity;
        var remainingDiscount = item.Discount;

        foreach (var stock in stocks)
        {
            if (remainingQuantity <= 0)
                break;

            var allocatedQuantity = Math.Min(stock.Quantity, remainingQuantity);
            var allocatedDiscount = allocatedQuantity == remainingQuantity
                ? remainingDiscount
                : decimal.Round(
                    item.Discount * allocatedQuantity / item.Quantity,
                    2,
                    MidpointRounding.AwayFromZero);

            stock.Quantity -= allocatedQuantity;
            stock.UpdatedAt = DateTime.UtcNow;
            stock.UpdateBy = userId;
            unitOfWork.InventoryStocks.Update(stock);

            await unitOfWork.SaleItems.AddAsync(new Domain.Models.SaleItem
            {
                SaleID = sale.Id,
                MedicineId = item.MedicineId,
                MedicineUnitId = stock.MedicineUnitId,
                InventoryBatchId = stock.InventoryBatchId,
                LocationId = stock.LocationId,
                Quantity = allocatedQuantity,
                UnitPrice = item.UnitPrice,
                Discount = allocatedDiscount,
                TotalPrice = (allocatedQuantity * item.UnitPrice) - allocatedDiscount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            });

            await unitOfWork.InventoryTransactions.AddAsync(new Domain.Models.InventoryTransaction
            {
                MedicineId = item.MedicineId,
                InventoryBatchId = stock.InventoryBatchId,
                MedicineUnitId = stock.MedicineUnitId,
                LocationId = stock.LocationId,
                Quantity = allocatedQuantity,
                UnitCost = item.UnitPrice,
                TotalCost = allocatedQuantity * item.UnitPrice,
                TransactionType = InventoryTransactionType.Sale,
                ReferenceType = InventoryReferenceType.Sale,
                ReferenceID = sale.Id,
                TransactionDate = DateTime.UtcNow,
                ReferenceNumber = sale.InvoiceNumber,
                Description = $"Sale #{sale.Id}",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            });

            remainingQuantity -= allocatedQuantity;
            remainingDiscount -= allocatedDiscount;
        }
    }
}
