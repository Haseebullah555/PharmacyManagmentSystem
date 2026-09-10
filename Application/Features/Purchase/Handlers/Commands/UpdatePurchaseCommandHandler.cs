using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Dtos.PurchaseItem;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Purchase.Requests.Commands
{
    public class UpdatePurchaseCommandHandler(IUnitOfWork unitOfWork, ICurrentUserRepository currentUser)
        : IRequestHandler<UpdatePurchaseCommand>
    {
        public async Task Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UpdatePurchaseDto;
            var userId = currentUser.GetCurrentLoggedInUserId();

            // ============================================================
            // VALIDATION
            // ============================================================

            if (dto.Items == null || !dto.Items.Any())
                throw new Exception(
                    "Purchase must contain at least one item.");

            if (dto.PaidAmount < 0)
                throw new Exception(
                    "Paid amount cannot be negative.");

            if (dto.Items.Any(x => x.Quantity <= 0))
                throw new Exception(
                    "Item quantity must be greater than zero.");

            if (dto.Items.Any(x => x.UnitPrice < 0))
                throw new Exception(
                    "Item unit price cannot be negative.");

            var totalAmount = dto.Items.Sum(x =>
                x.Quantity * x.UnitPrice);

            if (dto.PaidAmount > totalAmount)
                throw new Exception(
                    "Paid amount cannot be greater than total amount.");

            // ============================================================
            // BEGIN TRANSACTION
            // ============================================================

            await using var transaction =
                await unitOfWork.BeginTransactionAsync();

            try
            {
                // ========================================================
                // 1. GET EXISTING PURCHASE
                // ========================================================

                var purchase =
                    await unitOfWork.Purchases
                        .Query()
                        .FirstOrDefaultAsync(
                            x => x.Id == dto.Id,
                            cancellationToken);

                if (purchase == null)
                    throw new Exception(
                        "Purchase not found.");


                // ========================================================
                // 2. GET EXISTING PURCHASE ITEMS
                // ========================================================

                var existingItems =
                    await unitOfWork.PurchaseItems
                        .Query()
                        .Where(x => x.PurchaseId == purchase.Id)
                        .ToListAsync(cancellationToken);


                // ========================================================
                // 3. UPDATE PURCHASE HEADER
                // ========================================================

                purchase.PurchaseDate =
                    dto.PurchaseDate;

                purchase.InvoiceNumber =
                    dto.InvoiceNumber;

                purchase.SupplierId =
                    dto.SupplierId;

                purchase.CurrencyId =
                    dto.CurrencyId;

                purchase.ExchangeRate =
                    dto.ExchangeRate;

                purchase.TotalAmount =
                    totalAmount;

                purchase.PaidAmount =
                    dto.PaidAmount;

                purchase.UnpaidAmount =
                    totalAmount - dto.PaidAmount;

                purchase.Remarks =
                    dto.Remarks;

                purchase.UpdatedAt =
                    DateTime.UtcNow;

                purchase.UpdateBy =
                    userId;

                unitOfWork.Purchases.Update(purchase);


                // ========================================================
                // 4. FIND REMOVED ITEMS
                //
                // An existing DB item that is not included in the
                // submitted DTO has been removed by the user.
                // ========================================================

                var submittedItemIds =
                    dto.Items
                        .Where(x => x.Id.HasValue)
                        .Select(x => x.Id!.Value)
                        .ToHashSet();

                var removedItems =
                    existingItems
                        .Where(x =>
                            !submittedItemIds.Contains(x.Id))
                        .ToList();


                // ========================================================
                // 5. REMOVE DELETED PURCHASE ITEMS
                // ========================================================

                foreach (var oldItem in removedItems)
                {
                    // ----------------------------------------------------
                    // Find old inventory stock.
                    //
                    // PurchaseItem currently does not store LocationId.
                    // Therefore we find the matching stock by batch/unit.
                    // ----------------------------------------------------

                    var oldStock =
                        await unitOfWork.InventoryStocks
                            .Query()
                            .Where(x =>
                                x.InventoryBatchId ==
                                    oldItem.InventoryBatchId &&
                                x.MedicineUnitId ==
                                    oldItem.MedicineUnitId)
                            .FirstOrDefaultAsync(
                                cancellationToken);

                    if (oldStock == null)
                    {
                        throw new Exception(
                            $"Inventory stock not found for purchase item {oldItem.Id}.");
                    }


                    // ----------------------------------------------------
                    // Make sure the stock wasn't already consumed.
                    // ----------------------------------------------------

                    if (oldStock.Quantity < oldItem.Quantity)
                    {
                        throw new Exception(
                            $"Cannot remove purchase item {oldItem.Id} because part of its stock has already been used.");
                    }


                    // ----------------------------------------------------
                    // Reduce stock
                    // ----------------------------------------------------

                    oldStock.Quantity -=
                        oldItem.Quantity;

                    oldStock.UpdatedAt =
                        DateTime.UtcNow;

                    oldStock.UpdateBy =
                        userId;

                    unitOfWork.InventoryStocks
                        .Update(oldStock);


                    // ----------------------------------------------------
                    // Inventory transaction
                    // ----------------------------------------------------

                    var inventoryTransaction =
                        new InventoryTransaction
                        {
                            MedicineId =
                                oldItem.MedicineId,

                            InventoryBatchId =
                                oldItem.InventoryBatchId,

                            MedicineUnitId =
                                oldItem.MedicineUnitId,

                            LocationId =
                                oldStock.LocationId,

                            Quantity =
                                oldItem.Quantity,

                            TransactionType =
                                InventoryTransactionType.AdjustmentOut,

                            ReferenceType =
                                InventoryReferenceType.Purchase,

                            ReferenceID =
                                purchase.Id,

                            TransactionDate =
                                DateTime.UtcNow,

                            ReferenceNumber =
                                purchase.InvoiceNumber,

                            Description =
                                $"Purchase update - removed item #{oldItem.Id} from purchase #{purchase.Id}",

                            CreatedAt =
                                DateTime.UtcNow,

                            CreatedBy =
                                userId
                        };

                    await unitOfWork.InventoryTransactions
                        .AddAsync(
                            inventoryTransaction);


                    // ----------------------------------------------------
                    // Delete PurchaseItem
                    // ----------------------------------------------------

                    unitOfWork.PurchaseItems
                        .Delete(oldItem);
                }


                // ========================================================
                // 6. PROCESS SUBMITTED ITEMS
                // ========================================================

                foreach (var itemDto in dto.Items)
                {
                    // ====================================================
                    // 6.1 NEW ITEM
                    // ====================================================

                    if (!itemDto.Id.HasValue)
                    {
                        var batch =
                            await GetOrCreateBatch(
                                itemDto,
                                userId,
                                cancellationToken);


                        // ------------------------------------------------
                        // Create PurchaseItem
                        // ------------------------------------------------

                        var purchaseItem =
                            new PurchaseItem
                            {
                                PurchaseId =
                                    purchase.Id,

                                MedicineId =
                                    itemDto.MedicineId,

                                MedicineUnitId =
                                    itemDto.MedicineUnitId,

                                Quantity =
                                    itemDto.Quantity,

                                UnitPrice =
                                    itemDto.UnitPrice,

                                TotalPrice =
                                    itemDto.Quantity *
                                    itemDto.UnitPrice,

                                InventoryBatchId =
                                    batch.Id,

                                CreatedAt =
                                    DateTime.UtcNow,

                                CreatedBy =
                                    userId
                            };

                        await unitOfWork.PurchaseItems
                            .AddAsync(purchaseItem);


                        // ------------------------------------------------
                        // Increase inventory stock
                        // ------------------------------------------------

                        await IncreaseStock(
                            itemDto.MedicineId,
                            itemDto.MedicineUnitId,
                            batch.Id,
                            itemDto.LocationId,
                            itemDto.Quantity,
                            userId,
                            cancellationToken);


                        // ------------------------------------------------
                        // Inventory transaction
                        // ------------------------------------------------

                        var inventoryTransaction =
                            new InventoryTransaction
                            {
                                MedicineId =
                                    itemDto.MedicineId,

                                InventoryBatchId =
                                    batch.Id,

                                MedicineUnitId =
                                    itemDto.MedicineUnitId,

                                LocationId =
                                    itemDto.LocationId,

                                Quantity =
                                    itemDto.Quantity,

                                UnitCost =
                                    itemDto.UnitPrice,

                                TotalCost =
                                    itemDto.Quantity *
                                    itemDto.UnitPrice,

                                TransactionType =
                                    InventoryTransactionType.Purchase,

                                ReferenceType =
                                    InventoryReferenceType.Purchase,

                                ReferenceID =
                                    purchase.Id,

                                TransactionDate =
                                    DateTime.UtcNow,

                                ReferenceNumber =
                                    purchase.InvoiceNumber,

                                Description =
                                    $"Purchase update - added item to purchase #{purchase.Id}",

                                CreatedAt =
                                    DateTime.UtcNow,

                                CreatedBy =
                                    userId
                            };

                        await unitOfWork.InventoryTransactions
                            .AddAsync(
                                inventoryTransaction);

                        continue;
                    }


                    // ====================================================
                    // 6.2 EXISTING ITEM
                    // ====================================================

                    var oldItem =
                        existingItems.FirstOrDefault(
                            x => x.Id == itemDto.Id.Value);

                    if (oldItem == null)
                    {
                        throw new Exception(
                            $"Purchase item {itemDto.Id} was not found.");
                    }


                    // ----------------------------------------------------
                    // Find the existing stock
                    // ----------------------------------------------------

                    var oldStock =
                        await unitOfWork.InventoryStocks
                            .Query()
                            .Where(x =>
                                x.InventoryBatchId ==
                                    oldItem.InventoryBatchId &&
                                x.MedicineUnitId ==
                                    oldItem.MedicineUnitId)
                            .FirstOrDefaultAsync(
                                cancellationToken);

                    if (oldStock == null)
                    {
                        throw new Exception(
                            $"Inventory stock not found for purchase item {oldItem.Id}.");
                    }


                    var oldLocationId =
                        oldStock.LocationId;


                    // ====================================================
                    // GET NEW / EXISTING BATCH
                    // ====================================================

                    var newBatch =
                        await GetOrCreateBatch(itemDto, userId, cancellationToken);


                    // ====================================================
                    // DETERMINE WHAT CHANGED
                    // ====================================================

                    var batchChanged =
                        oldItem.InventoryBatchId !=
                        newBatch.Id;

                    var medicineChanged =
                        oldItem.MedicineId !=
                        itemDto.MedicineId;

                    var unitChanged =
                        oldItem.MedicineUnitId !=
                        itemDto.MedicineUnitId;

                    var locationChanged =
                        oldLocationId !=
                        itemDto.LocationId;


                    // ====================================================
                    // 6.3 BATCH / MEDICINE / UNIT / LOCATION CHANGED
                    // ====================================================

                    if (batchChanged ||
                        medicineChanged ||
                        unitChanged ||
                        locationChanged)
                    {
                        // ------------------------------------------------
                        // The complete old quantity needs to be removed
                        // from the old stock.
                        // ------------------------------------------------

                        if (oldStock.Quantity < oldItem.Quantity)
                        {
                            throw new Exception(
                                $"Cannot change purchase item {oldItem.Id} because part of its stock has already been used.");
                        }


                        // ------------------------------------------------
                        // Remove old stock
                        // ------------------------------------------------

                        oldStock.Quantity -=
                            oldItem.Quantity;

                        oldStock.UpdatedAt =
                            DateTime.UtcNow;

                        oldStock.UpdateBy =
                            userId;

                        unitOfWork.InventoryStocks
                            .Update(oldStock);


                        // ------------------------------------------------
                        // Inventory transaction - remove old stock
                        // ------------------------------------------------

                        var adjustmentTransaction =
                            new InventoryTransaction
                            {
                                MedicineId =
                                    oldItem.MedicineId,

                                InventoryBatchId =
                                    oldItem.InventoryBatchId,

                                MedicineUnitId =
                                    oldItem.MedicineUnitId,

                                LocationId =
                                    oldStock.LocationId,

                                Quantity =
                                    oldItem.Quantity,

                                TransactionType =
                                    InventoryTransactionType.AdjustmentOut,

                                ReferenceType =
                                    InventoryReferenceType.Purchase,

                                ReferenceID =
                                    purchase.Id,

                                TransactionDate =
                                    DateTime.UtcNow,

                                ReferenceNumber =
                                    purchase.InvoiceNumber,

                                Description =
                                    $"Purchase update - removed old stock for item #{oldItem.Id}",

                                CreatedAt =
                                    DateTime.UtcNow,

                                CreatedBy =
                                    userId
                            };

                        await unitOfWork.InventoryTransactions
                            .AddAsync(
                                adjustmentTransaction);


                        // ------------------------------------------------
                        // Add new stock
                        // ------------------------------------------------

                        await IncreaseStock(
                            itemDto.MedicineId,
                            itemDto.MedicineUnitId,
                            newBatch.Id,
                            itemDto.LocationId,
                            itemDto.Quantity,
                            userId,
                            cancellationToken);


                        // ------------------------------------------------
                        // Inventory transaction - add new stock
                        // ------------------------------------------------

                        var purchaseTransaction =
                            new InventoryTransaction
                            {
                                MedicineId =
                                    itemDto.MedicineId,

                                InventoryBatchId =
                                    newBatch.Id,

                                MedicineUnitId =
                                    itemDto.MedicineUnitId,

                                LocationId =
                                    itemDto.LocationId,

                                Quantity =
                                    itemDto.Quantity,

                                UnitCost =
                                    itemDto.UnitPrice,

                                TotalCost =
                                    itemDto.Quantity *
                                    itemDto.UnitPrice,

                                TransactionType =
                                    InventoryTransactionType.Purchase,

                                ReferenceType =
                                    InventoryReferenceType.Purchase,

                                ReferenceID =
                                    purchase.Id,

                                TransactionDate =
                                    DateTime.UtcNow,

                                ReferenceNumber =
                                    purchase.InvoiceNumber,

                                Description =
                                    $"Purchase update - added new stock for item #{oldItem.Id}",

                                CreatedAt =
                                    DateTime.UtcNow,

                                CreatedBy =
                                    userId
                            };

                        await unitOfWork.InventoryTransactions
                            .AddAsync(
                                purchaseTransaction);
                    }
                    else
                    {
                        // =================================================
                        // SAME BATCH / MEDICINE / UNIT / LOCATION
                        // =================================================

                        var quantityDifference =
                            itemDto.Quantity -
                            oldItem.Quantity;


                        // ================================================
                        // QUANTITY DECREASED
                        // ================================================

                        if (quantityDifference < 0)
                        {
                            var quantityToRemove =
                                Math.Abs(quantityDifference);

                            if (oldStock.Quantity <
                                quantityToRemove)
                            {
                                throw new Exception(
                                    $"Cannot reduce purchase item {oldItem.Id} because the required stock has already been used.");
                            }


                            oldStock.Quantity -=
                                quantityToRemove;

                            oldStock.UpdatedAt =
                                DateTime.UtcNow;

                            oldStock.UpdateBy =
                                userId;

                            unitOfWork.InventoryStocks
                                .Update(oldStock);


                            // --------------------------------------------
                            // Adjustment Out transaction
                            // --------------------------------------------

                            var adjustmentTransaction =
                                new InventoryTransaction
                                {
                                    MedicineId =
                                        oldItem.MedicineId,

                                    InventoryBatchId =
                                        oldItem.InventoryBatchId,

                                    MedicineUnitId =
                                        oldItem.MedicineUnitId,

                                    LocationId =
                                        oldStock.LocationId,

                                    Quantity =
                                        quantityToRemove,

                                    TransactionType =
                                        InventoryTransactionType.AdjustmentOut,

                                    ReferenceType =
                                        InventoryReferenceType.Purchase,

                                    ReferenceID =
                                        purchase.Id,

                                    TransactionDate =
                                        DateTime.UtcNow,

                                    ReferenceNumber =
                                        purchase.InvoiceNumber,

                                    Description =
                                        $"Purchase update - reduced quantity for item #{oldItem.Id}",

                                    CreatedAt =
                                        DateTime.UtcNow,

                                    CreatedBy =
                                        userId
                                };

                            await unitOfWork.InventoryTransactions
                                .AddAsync(
                                    adjustmentTransaction);
                        }


                        // ================================================
                        // QUANTITY INCREASED
                        // ================================================

                        else if (quantityDifference > 0)
                        {
                            oldStock.Quantity +=
                                quantityDifference;

                            oldStock.UpdatedAt =
                                DateTime.UtcNow;

                            oldStock.UpdateBy =
                                userId;

                            unitOfWork.InventoryStocks
                                .Update(oldStock);


                            // --------------------------------------------
                            // Purchase transaction
                            // --------------------------------------------

                            var purchaseTransaction =
                                new InventoryTransaction
                                {
                                    MedicineId =
                                        itemDto.MedicineId,

                                    InventoryBatchId =
                                        newBatch.Id,

                                    MedicineUnitId =
                                        itemDto.MedicineUnitId,

                                    LocationId =
                                        oldStock.LocationId,

                                    Quantity =
                                        quantityDifference,

                                    UnitCost =
                                        itemDto.UnitPrice,

                                    TotalCost =
                                        quantityDifference *
                                        itemDto.UnitPrice,

                                    TransactionType =
                                        InventoryTransactionType.Purchase,

                                    ReferenceType =
                                        InventoryReferenceType.Purchase,

                                    ReferenceID =
                                        purchase.Id,

                                    TransactionDate =
                                        DateTime.UtcNow,

                                    ReferenceNumber =
                                        purchase.InvoiceNumber,

                                    Description =
                                        $"Purchase update - increased quantity for item #{oldItem.Id}",

                                    CreatedAt =
                                        DateTime.UtcNow,

                                    CreatedBy =
                                        userId
                                };

                            await unitOfWork.InventoryTransactions
                                .AddAsync(
                                    purchaseTransaction);
                        }
                    }


                    // ====================================================
                    // UPDATE BATCH INFORMATION
                    // ====================================================

                    newBatch.ManufacturingDate =
                        itemDto.ManufacturingDate;

                    newBatch.ExpiryDate =
                        itemDto.ExpiryDate;

                    newBatch.IsActive = true;

                    newBatch.UpdatedAt =
                        DateTime.UtcNow;

                    newBatch.UpdateBy =
                        userId;

                    unitOfWork.InventoryBatches
                        .Update(newBatch);


                    // ====================================================
                    // UPDATE PURCHASE ITEM
                    // ====================================================

                    oldItem.MedicineId =
                        itemDto.MedicineId;

                    oldItem.MedicineUnitId =
                        itemDto.MedicineUnitId;

                    oldItem.Quantity =
                        itemDto.Quantity;

                    oldItem.UnitPrice =
                        itemDto.UnitPrice;

                    oldItem.TotalPrice =
                        itemDto.Quantity *
                        itemDto.UnitPrice;

                    oldItem.InventoryBatchId =
                        newBatch.Id;

                    oldItem.UpdatedAt =
                        DateTime.UtcNow;

                    oldItem.UpdateBy =
                        userId;

                    unitOfWork.PurchaseItems
                        .Update(oldItem);
                }


                // ========================================================
                // 7. SAVE EVERYTHING
                // ========================================================

                await unitOfWork.SaveAsync(
                    cancellationToken);


                // ========================================================
                // 8. COMMIT TRANSACTION
                // ========================================================

                await transaction.CommitAsync(
                    cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(
                    cancellationToken);

                throw;
            }
        }


        // =================================================================
        // GET EXISTING BATCH OR CREATE NEW BATCH
        // =================================================================

        private async Task<InventoryBatch> GetOrCreateBatch(
            UpdatePurchaseItemDto itemDto,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var batch =
                await unitOfWork.InventoryBatches
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.MedicineId ==
                                itemDto.MedicineId &&
                            x.BatchNumber ==
                                itemDto.BatchNumber,
                        cancellationToken);

            if (batch != null)
                return batch;


            batch = new InventoryBatch
            {
                MedicineId =
                    itemDto.MedicineId,

                BatchNumber =
                    itemDto.BatchNumber,

                ManufacturingDate =
                    itemDto.ManufacturingDate,

                ExpiryDate =
                    itemDto.ExpiryDate,

                IsActive = true,

                CreatedAt =
                    DateTime.UtcNow,

                CreatedBy =
                    userId
            };

            await unitOfWork.InventoryBatches
                .AddAsync(batch);

            await unitOfWork.SaveAsync(
                cancellationToken);

            return batch;
        }


        // =================================================================
        // INCREASE INVENTORY STOCK
        // =================================================================

        private async Task IncreaseStock(
            int medicineId,
            int medicineUnitId,
            int inventoryBatchId,
            int locationId,
            decimal quantity,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var stock =
                await unitOfWork.InventoryStocks
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.InventoryBatchId ==
                                inventoryBatchId &&
                            x.LocationId ==
                                locationId &&
                            x.MedicineUnitId ==
                                medicineUnitId,
                        cancellationToken);

            if (stock == null)
            {
                stock = new InventoryStock
                {
                    InventoryBatchId =
                        inventoryBatchId,

                    LocationId =
                        locationId,

                    MedicineUnitId =
                        medicineUnitId,

                    Quantity =
                        quantity,

                    CreatedAt =
                        DateTime.UtcNow,

                    CreatedBy =
                        userId
                };

                await unitOfWork.InventoryStocks
                    .AddAsync(stock);
            }
            else
            {
                stock.Quantity +=
                    quantity;

                stock.UpdatedAt =
                    DateTime.UtcNow;

                stock.UpdateBy =
                    userId;

                unitOfWork.InventoryStocks
                    .Update(stock);
            }
        }
    }
}