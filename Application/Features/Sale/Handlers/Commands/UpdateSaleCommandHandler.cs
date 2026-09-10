using System.ComponentModel.DataAnnotations;
using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Dtos.SaleItem;
using Application.Features.Sale.Requests.Commands;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateSaleCommandHandler(IUnitOfWork unitOfWork, ICurrentUserRepository currentUser) : IRequestHandler<UpdateSaleCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserRepository _currentUser = currentUser;

    public async Task Handle( UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.UpdateSaleDto;

        // ---------------------------------------------------------
        // 1. Validate request
        // ---------------------------------------------------------

        if (dto.Items == null || !dto.Items.Any())
            throw new Exception("Sale must contain at least one item.");

        if (dto.PaidAmount < 0)
            throw new Exception("Paid amount cannot be negative.");

        if (dto.Discount < 0)
            throw new Exception("Discount cannot be negative.");

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0)
                throw new Exception("Sale quantity must be greater than zero.");

            if (item.UnitPrice < 0)
                throw new Exception("Unit price cannot be negative.");

            if (item.Discount < 0)
                throw new Exception("Item discount cannot be negative.");

            var itemGross = item.Quantity * item.UnitPrice;

            if (item.Discount > itemGross)
                throw new Exception(
                    "Item discount cannot be greater than item total.");
        }

        var itemsTotal = dto.Items.Sum(item =>
            (item.Quantity * item.UnitPrice) - item.Discount);

        var totalAmount = itemsTotal - dto.Discount;

        if (totalAmount < 0)
            throw new Exception(
                "Discount cannot be greater than the total amount.");

        if (dto.PaidAmount > totalAmount)
            throw new Exception(
                "Paid amount cannot be greater than the total amount.");

        // ---------------------------------------------------------
        // 2. Begin transaction
        // ---------------------------------------------------------

        await using var transaction =
            await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId =
                _currentUser.GetCurrentLoggedInUserId();

            // ---------------------------------------------------------
            // 3. Load existing Sale
            // ---------------------------------------------------------

            var sale = await _unitOfWork.Sales
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == dto.Id,
                    cancellationToken);

            if (sale == null)
                throw new Exception(
                    $"Sale with ID {dto.Id} was not found.");

            // ---------------------------------------------------------
            // 4. Load existing Sale Items
            // ---------------------------------------------------------

            var existingItems = await _unitOfWork.SaleItems
                .Query()
                .Where(x => x.SaleID == sale.Id)
                .ToListAsync(cancellationToken);

            // ---------------------------------------------------------
            // 5. Update Sale Header
            // ---------------------------------------------------------

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

            _unitOfWork.Sales.Update(sale);

            // IDs of items that still exist after update
            var submittedExistingIds = dto.Items
                .Where(x => x.Id.HasValue)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            // ---------------------------------------------------------
            // 6. Remove deleted Sale Items
            // ---------------------------------------------------------

            var removedItems = existingItems
                .Where(x => !submittedExistingIds.Contains(x.Id))
                .ToList();

            foreach (var oldItem in removedItems)
            {
                // Restore quantity to the original stock.
                var oldStock = await GetStock(
                    oldItem.InventoryBatchId,
                    oldItem.LocationId,
                    oldItem.MedicineUnitId,
                    cancellationToken);

                if (oldStock == null)
                {
                    throw new Exception(
                        $"Inventory stock was not found for deleted sale item {oldItem.Id}.");
                }

                oldStock.Quantity += oldItem.Quantity;
                oldStock.UpdatedAt = DateTime.UtcNow;
                oldStock.UpdateBy = userId;

                _unitOfWork.InventoryStocks.Update(oldStock);

                // Record inventory restoration.
                await AddInventoryTransaction(
                    medicineId: oldItem.MedicineId,
                    inventoryBatchId: oldItem.InventoryBatchId,
                    medicineUnitId: oldItem.MedicineUnitId,
                    locationId: oldItem.LocationId,
                    quantity: oldItem.Quantity,
                    transactionType: InventoryTransactionType.AdjustmentIn,
                    saleId: sale.Id,
                    invoiceNumber: sale.InvoiceNumber,
                    unitCost: oldItem.UnitPrice,
                    totalCost: oldItem.Quantity * oldItem.UnitPrice,
                    description: $"Sale item #{oldItem.Id} removed from Sale #{sale.Id}",
                    userId: userId);

                _unitOfWork.SaleItems.Delete(oldItem);
            }

            // ---------------------------------------------------------
            // 7. Process submitted items
            // ---------------------------------------------------------

            foreach (var item in dto.Items)
            {
                // =====================================================
                // NEW ITEM
                // =====================================================

                if (!item.Id.HasValue)
                {
                    await AddNewSaleItem(
                        sale,
                        item,
                        userId,
                        cancellationToken);

                    continue;
                }

                // =====================================================
                // EXISTING ITEM
                // =====================================================

                var existingItem = existingItems
                    .FirstOrDefault(x => x.Id == item.Id.Value);

                if (existingItem == null)
                {
                    throw new Exception(
                        $"Sale item with ID {item.Id.Value} was not found.");
                }

                var inventoryChanged =
                    existingItem.MedicineId != item.MedicineId ||
                    existingItem.MedicineUnitId != item.MedicineUnitId ||
                    existingItem.InventoryBatchId != item.InventoryBatchId ||
                    existingItem.LocationId != item.LocationId;

                // -----------------------------------------------------
                // Same inventory location/batch/unit
                // Only quantity may need adjustment
                // -----------------------------------------------------

                if (!inventoryChanged)
                {
                    var quantityDifference =
                        item.Quantity - existingItem.Quantity;

                    if (quantityDifference > 0)
                    {
                        // User increased sale quantity.
                        // Consume additional stock.

                        var stock = await GetStock(
                            item.InventoryBatchId,
                            item.LocationId,
                            item.MedicineUnitId,
                            cancellationToken);

                        if (stock == null)
                        {
                            throw new Exception(
                                "Inventory stock was not found.");
                        }

                        if (stock.Quantity < quantityDifference)
                        {
                            throw new ValidationException(
                                $"Insufficient stock. Available: {stock.Quantity}, " +
                                $"Requested: {quantityDifference}.");
                        }

                        stock.Quantity -= quantityDifference;
                        stock.UpdatedAt = DateTime.UtcNow;
                        stock.UpdateBy = userId;

                        _unitOfWork.InventoryStocks.Update(stock);

                        await AddInventoryTransaction(
                            medicineId: item.MedicineId,
                            inventoryBatchId: item.InventoryBatchId,
                            medicineUnitId: item.MedicineUnitId,
                            locationId: item.LocationId,
                            quantity: quantityDifference,
                            transactionType: InventoryTransactionType.Sale,
                            saleId: sale.Id,
                            invoiceNumber: sale.InvoiceNumber,
                            unitCost: item.UnitPrice,
                            totalCost: quantityDifference * item.UnitPrice,
                            description: $"Additional quantity for Sale #{sale.Id}",
                            userId: userId);
                    }
                    else if (quantityDifference < 0)
                    {
                        // User decreased sale quantity.
                        // Return the difference to stock.

                        var returnedQuantity =
                            Math.Abs(quantityDifference);

                        var stock = await GetStock(
                            item.InventoryBatchId,
                            item.LocationId,
                            item.MedicineUnitId,
                            cancellationToken);

                        if (stock == null)
                        {
                            throw new Exception(
                                "Inventory stock was not found.");
                        }

                        stock.Quantity += returnedQuantity;
                        stock.UpdatedAt = DateTime.UtcNow;
                        stock.UpdateBy = userId;

                        _unitOfWork.InventoryStocks.Update(stock);

                        await AddInventoryTransaction(
                            medicineId: existingItem.MedicineId,
                            inventoryBatchId: existingItem.InventoryBatchId,
                            medicineUnitId: existingItem.MedicineUnitId,
                            locationId: existingItem.LocationId,
                            quantity: returnedQuantity,
                            transactionType: InventoryTransactionType.AdjustmentIn,
                            saleId: sale.Id,
                            invoiceNumber: sale.InvoiceNumber,
                            unitCost: existingItem.UnitPrice,
                            totalCost: returnedQuantity * existingItem.UnitPrice,
                            description: $"Quantity reduced for Sale #{sale.Id}",
                            userId: userId);
                    }
                }
                else
                {
                    // -------------------------------------------------
                    // Inventory identity changed.
                    //
                    // Example:
                    // Batch A / Location A -> Batch B / Location B
                    //
                    // Restore old stock first, then consume new stock.
                    // -------------------------------------------------

                    var oldStock = await GetStock(
                        existingItem.InventoryBatchId,
                        existingItem.LocationId,
                        existingItem.MedicineUnitId,
                        cancellationToken);

                    if (oldStock == null)
                    {
                        throw new Exception(
                            $"Original inventory stock was not found for Sale item {existingItem.Id}.");
                    }

                    // Restore old sale quantity.
                    oldStock.Quantity += existingItem.Quantity;
                    oldStock.UpdatedAt = DateTime.UtcNow;
                    oldStock.UpdateBy = userId;

                    _unitOfWork.InventoryStocks.Update(oldStock);

                    await AddInventoryTransaction(
                        medicineId: existingItem.MedicineId,
                        inventoryBatchId: existingItem.InventoryBatchId,
                        medicineUnitId: existingItem.MedicineUnitId,
                        locationId: existingItem.LocationId,
                        quantity: existingItem.Quantity,
                        transactionType: InventoryTransactionType.AdjustmentIn,
                        saleId: sale.Id,
                        invoiceNumber: sale.InvoiceNumber,
                        unitCost: existingItem.UnitPrice,
                        totalCost: existingItem.Quantity * existingItem.UnitPrice,
                        description: $"Original inventory restored for Sale #{sale.Id}",
                        userId: userId);

                    // Validate the new inventory batch.
                    await ValidateInventoryBatch(
                        item,
                        cancellationToken);

                    // Find new stock.
                    var newStock = await GetStock(
                        item.InventoryBatchId,
                        item.LocationId,
                        item.MedicineUnitId,
                        cancellationToken);

                    if (newStock == null)
                    {
                        throw new Exception(
                            "New inventory stock was not found.");
                    }

                    if (newStock.Quantity < item.Quantity)
                    {
                        throw new ValidationException(
                            $"Insufficient stock. Available: {newStock.Quantity}, " +
                            $"Requested: {item.Quantity}.");
                    }

                    // Consume new stock.
                    newStock.Quantity -= item.Quantity;
                    newStock.UpdatedAt = DateTime.UtcNow;
                    newStock.UpdateBy = userId;

                    _unitOfWork.InventoryStocks.Update(newStock);

                    await AddInventoryTransaction(
                        medicineId: item.MedicineId,
                        inventoryBatchId: item.InventoryBatchId,
                        medicineUnitId: item.MedicineUnitId,
                        locationId: item.LocationId,
                        quantity: item.Quantity,
                        transactionType: InventoryTransactionType.Sale,
                        saleId: sale.Id,
                        invoiceNumber: sale.InvoiceNumber,
                        unitCost: item.UnitPrice,
                        totalCost: item.Quantity * item.UnitPrice,
                        description: $"Inventory changed for Sale #{sale.Id}",
                        userId: userId);
                }

                // -----------------------------------------------------
                // Update SaleItem
                // -----------------------------------------------------

                existingItem.MedicineId = item.MedicineId;
                existingItem.MedicineUnitId = item.MedicineUnitId;
                existingItem.InventoryBatchId = item.InventoryBatchId;
                existingItem.LocationId = item.LocationId;

                existingItem.Quantity = item.Quantity;
                existingItem.UnitPrice = item.UnitPrice;
                existingItem.Discount = item.Discount;

                existingItem.TotalPrice =
                    (item.Quantity * item.UnitPrice) - item.Discount;

                existingItem.UpdatedAt = DateTime.UtcNow;
                existingItem.UpdateBy = userId;

                _unitOfWork.SaleItems.Update(existingItem);
            }

            // ---------------------------------------------------------
            // 8. Save everything
            // ---------------------------------------------------------

            await _unitOfWork.SaveAsync(cancellationToken);

            // ---------------------------------------------------------
            // 9. Commit
            // ---------------------------------------------------------

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    // =============================================================
    // Add New Sale Item
    // =============================================================

    private async Task AddNewSaleItem(
        Domain.Models.Sale sale,
        UpdateSaleItemDto item,
        Guid userId,
        CancellationToken cancellationToken)
    {
        // Validate batch.
        await ValidateInventoryBatch(
            item,
            cancellationToken);

        // Find stock.
        var stock = await GetStock(
            item.InventoryBatchId,
            item.LocationId,
            item.MedicineUnitId,
            cancellationToken);

        if (stock == null)
        {
            throw new Exception(
                $"Inventory stock was not found for Medicine ID {item.MedicineId}.");
        }

        // Check available quantity.
        if (stock.Quantity < item.Quantity)
        {
            throw new ValidationException(
                $"Insufficient stock. Available: {stock.Quantity}, " +
                $"Requested: {item.Quantity}.");
        }

        // Consume stock.
        stock.Quantity -= item.Quantity;
        stock.UpdatedAt = DateTime.UtcNow;
        stock.UpdateBy = userId;

        _unitOfWork.InventoryStocks.Update(stock);

        // Create SaleItem.
        var saleItem = new Domain.Models.SaleItem
        {
            SaleID = sale.Id,

            MedicineId = item.MedicineId,
            MedicineUnitId = item.MedicineUnitId,
            InventoryBatchId = item.InventoryBatchId,
            LocationId = item.LocationId,

            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            Discount = item.Discount,

            TotalPrice =
                (item.Quantity * item.UnitPrice) - item.Discount,

            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _unitOfWork.SaleItems.AddAsync(saleItem);

        // Create inventory transaction.
        await AddInventoryTransaction(
            medicineId: item.MedicineId,
            inventoryBatchId: item.InventoryBatchId,
            medicineUnitId: item.MedicineUnitId,
            locationId: item.LocationId,
            quantity: item.Quantity,
            transactionType: InventoryTransactionType.Sale,
            saleId: sale.Id,
            invoiceNumber: sale.InvoiceNumber,
            unitCost: item.UnitPrice,
            totalCost: item.Quantity * item.UnitPrice,
            description: $"New item added to Sale #{sale.Id}",
            userId: userId);
    }

    // =============================================================
    // Validate Inventory Batch
    // =============================================================

    private async Task ValidateInventoryBatch(
        UpdateSaleItemDto item,
        CancellationToken cancellationToken)
    {
        var batch = await _unitOfWork.InventoryBatches
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x =>
                    x.Id == item.InventoryBatchId &&
                    x.MedicineId == item.MedicineId &&
                    x.IsActive,
                cancellationToken);

        if (batch == null)
        {
            throw new Exception(
                $"Inventory batch was not found for Medicine ID {item.MedicineId}.");
        }
    }

    // =============================================================
    // Get Inventory Stock
    // =============================================================

    private async Task<Domain.Models.InventoryStock?> GetStock(
        int inventoryBatchId,
        int locationId,
        int medicineUnitId,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.InventoryStocks
            .Query()
            .FirstOrDefaultAsync(
                x =>
                    x.InventoryBatchId == inventoryBatchId &&
                    x.LocationId == locationId &&
                    x.MedicineUnitId == medicineUnitId,
                cancellationToken);
    }

    // =============================================================
    // Add Inventory Transaction
    // =============================================================

    private async Task AddInventoryTransaction(
        int medicineId,
        int inventoryBatchId,
        int medicineUnitId,
        int locationId,
        decimal quantity,
        InventoryTransactionType transactionType,
        int saleId,
        string? invoiceNumber,
        decimal unitCost,
        decimal totalCost,
        string description,
        Guid userId)
    {
        var inventoryTransaction =
            new Domain.Models.InventoryTransaction
            {
                MedicineId = medicineId,
                InventoryBatchId = inventoryBatchId,
                MedicineUnitId = medicineUnitId,
                LocationId = locationId,

                Quantity = quantity,

                UnitCost = unitCost,
                TotalCost = totalCost,

                TransactionType = transactionType,

                ReferenceType = InventoryReferenceType.Sale,
                ReferenceID = saleId,

                TransactionDate = DateTime.UtcNow,

                ReferenceNumber = invoiceNumber,
                Description = description,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

        await _unitOfWork.InventoryTransactions
            .AddAsync(inventoryTransaction);
    }
}