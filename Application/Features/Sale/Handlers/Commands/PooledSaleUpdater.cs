using System.ComponentModel.DataAnnotations;
using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Dtos.Sale;
using Application.Dtos.SaleItem;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

internal static class PooledSaleUpdater
{
    internal static async Task UpdateAsync(
        IUnitOfWork unitOfWork,
        ICurrentUserRepository currentUser,
        UpdateSaleDto dto,
        CancellationToken cancellationToken)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new ValidationException("Sale must contain at least one item.");
        if (dto.PaidAmount < 0 || dto.Discount < 0)
            throw new ValidationException("Payment and discount cannot be negative.");

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0 || item.UnitPrice < 0 || item.Discount < 0 ||
                item.Discount > item.Quantity * item.UnitPrice)
                throw new ValidationException("Sale item values are invalid.");
        }

        var totalAmount = dto.Items.Sum(item =>
            (item.Quantity * item.UnitPrice) - item.Discount) - dto.Discount;
        if (totalAmount < 0 || dto.PaidAmount > totalAmount)
            throw new ValidationException("The payment or discount amount is invalid.");

        await using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var userId = currentUser.GetCurrentLoggedInUserId();
            var sale = await unitOfWork.Sales.Query()
                .FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken)
                ?? throw new ValidationException($"Sale with ID {dto.Id} was not found.");

            var existingItems = await unitOfWork.SaleItems.Query()
                .Where(item => item.SaleID == sale.Id)
                .ToListAsync(cancellationToken);

            // Restore all previous allocations, then allocate the submitted
            // medicine quantities again from the pooled stock.
            foreach (var oldItem in existingItems)
            {
                var stock = await unitOfWork.InventoryStocks.Query()
                    .FirstOrDefaultAsync(x =>
                        x.InventoryBatchId == oldItem.InventoryBatchId &&
                        x.LocationId == oldItem.LocationId &&
                        x.MedicineUnitId == oldItem.MedicineUnitId,
                        cancellationToken)
                    ?? throw new ValidationException(
                        $"Inventory stock was not found for sale item {oldItem.Id}.");

                stock.Quantity += oldItem.Quantity;
                stock.UpdatedAt = DateTime.UtcNow;
                stock.UpdateBy = userId;
                unitOfWork.InventoryStocks.Update(stock);

                await unitOfWork.InventoryTransactions.AddAsync(new Domain.Models.InventoryTransaction
                {
                    MedicineId = oldItem.MedicineId,
                    InventoryBatchId = oldItem.InventoryBatchId,
                    MedicineUnitId = oldItem.MedicineUnitId,
                    LocationId = oldItem.LocationId,
                    Quantity = oldItem.Quantity,
                    UnitCost = oldItem.UnitPrice,
                    TotalCost = oldItem.Quantity * oldItem.UnitPrice,
                    TransactionType = InventoryTransactionType.AdjustmentIn,
                    ReferenceType = InventoryReferenceType.Sale,
                    ReferenceID = sale.Id,
                    TransactionDate = DateTime.UtcNow,
                    ReferenceNumber = dto.InvoiceNumber,
                    Description = $"Sale #{sale.Id} reallocated",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                });

                unitOfWork.SaleItems.Delete(oldItem);
            }

            sale.SaleDate = dto.SaleDate;
            sale.CustomerId = dto.CustomerId;
            sale.CurrencyId = dto.CurrencyId;
            sale.TotalAmount = totalAmount;
            sale.PaidAmount = dto.PaidAmount;
            sale.UnpaidAmount = totalAmount - dto.PaidAmount;
            sale.Discount = dto.Discount;
            sale.InvoiceNumber = dto.InvoiceNumber;
            sale.Remarks = dto.Remarks;
            sale.UpdatedAt = DateTime.UtcNow;
            sale.UpdateBy = userId;
            unitOfWork.Sales.Update(sale);

            foreach (var item in dto.Items)
            {
                await PooledSaleAllocator.AllocateAsync(unitOfWork, sale,
                    new AddSaleItemDto
                    {
                        MedicineId = item.MedicineId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount
                    }, userId, cancellationToken);
            }

            await unitOfWork.SaveAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
