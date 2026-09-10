using System.ComponentModel.DataAnnotations;
using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Sale.Requests.Commands;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class AddSaleCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserRepository currentUser)
    : IRequestHandler<AddSaleCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserRepository _currentUser = currentUser;

    public async Task Handle(
        AddSaleCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.AddSaleDto;

        // ============================================================
        // VALIDATION
        // ============================================================

        if (dto.Items == null || !dto.Items.Any())
            throw new Exception(
                "Sale must contain at least one item.");

        if (dto.PaidAmount < 0)
            throw new Exception(
                "Paid amount cannot be negative.");

        if (dto.Discount < 0)
            throw new Exception(
                "Discount cannot be negative.");

        // ------------------------------------------------------------
        // Calculate items total after item-level discounts
        // ------------------------------------------------------------

        var itemsTotal = dto.Items.Sum(item =>
        {
            if (item.Quantity <= 0)
                throw new Exception(
                    "Sale quantity must be greater than zero.");

            if (item.UnitPrice < 0)
                throw new Exception(
                    "Unit price cannot be negative.");

            if (item.Discount < 0)
                throw new Exception(
                    "Item discount cannot be negative.");

            var itemGross =
                item.Quantity * item.UnitPrice;

            if (item.Discount > itemGross)
                throw new Exception(
                    "Item discount cannot be greater than item total.");

            return itemGross - item.Discount;
        });


        // ------------------------------------------------------------
        // Calculate final total after sale-level discount
        // ------------------------------------------------------------

        var totalAmount =
            itemsTotal - dto.Discount;

        if (totalAmount < 0)
            throw new Exception(
                "Discount cannot be greater than the total amount.");

        if (dto.PaidAmount > totalAmount)
            throw new Exception(
                "Paid amount cannot be greater than the total amount.");


        // ============================================================
        // BEGIN TRANSACTION
        // ============================================================

        await using var transaction =
            await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId =
                _currentUser.GetCurrentLoggedInUserId();


            // ========================================================
            // 1. CREATE SALE
            // ========================================================

            var sale =
                new Domain.Models.Sale
                {
                    SaleDate =
                        dto.SaleDate,

                    CustomerId =
                        dto.CustomerId,

                    CurrencyId =
                        dto.CurrencyId,

                    TotalAmount =
                        totalAmount,

                    PaidAmount =
                        dto.PaidAmount,

                    UnpaidAmount =
                        totalAmount - dto.PaidAmount,

                    Discount =
                        dto.Discount,

                    InvoiceNumber =
                        dto.InvoiceNumber,

                    Remarks =
                        dto.Remarks,

                    CreatedAt =
                        DateTime.UtcNow,

                    CreatedBy =
                        userId
                };

            await _unitOfWork.Sales
                .AddAsync(sale);

            // Generate Sale ID
            await _unitOfWork.SaveAsync(
                cancellationToken);


            // ========================================================
            // 2. PROCESS SALE ITEMS
            // ========================================================

            foreach (var item in dto.Items)
            {
                // ----------------------------------------------------
                // Find Inventory Batch
                // ----------------------------------------------------

                var batch =
                    await _unitOfWork.InventoryBatches
                        .Query()
                        .FirstOrDefaultAsync(
                            x =>
                                x.Id ==
                                    item.InventoryBatchId &&
                                x.MedicineId ==
                                    item.MedicineId &&
                                x.IsActive,
                            cancellationToken);

                if (batch == null)
                {
                    throw new Exception(
                        $"Inventory batch not found for Medicine ID {item.MedicineId}.");
                }


                // ----------------------------------------------------
                // Find Inventory Stock
                // ----------------------------------------------------

                var stock =
                    await _unitOfWork.InventoryStocks
                        .Query()
                        .FirstOrDefaultAsync(
                            x =>
                                x.InventoryBatchId ==
                                    item.InventoryBatchId &&

                                x.LocationId ==
                                    item.LocationId &&

                                x.MedicineUnitId ==
                                    item.MedicineUnitId,
                            cancellationToken);


                // ----------------------------------------------------
                // Check stock exists
                // ----------------------------------------------------

                if (stock == null)
                {
                    throw new Exception(
                        $"No inventory stock found for Medicine ID {item.MedicineId}.");
                }


                // ----------------------------------------------------
                // Check available quantity
                // ----------------------------------------------------

                if (stock.Quantity < item.Quantity)
                {
                    throw new ValidationException(
                        $"Insufficient stock. " +
                        $"Available: {stock.Quantity}, " +
                        $"Requested: {item.Quantity}.");
                }


                // ====================================================
                // 3. REDUCE INVENTORY STOCK
                // ====================================================

                stock.Quantity -=
                    item.Quantity;

                stock.UpdatedAt =
                    DateTime.UtcNow;

                stock.UpdateBy =
                    userId;

                _unitOfWork.InventoryStocks
                    .Update(stock);


                // ====================================================
                // 4. CREATE SALE ITEM
                // ====================================================

                var saleItem =
                    new Domain.Models.SaleItem
                    {
                        SaleID =
                            sale.Id,

                        MedicineId =
                            item.MedicineId,

                        MedicineUnitId =
                            item.MedicineUnitId,

                        InventoryBatchId =
                            item.InventoryBatchId,

                        LocationId =
                            item.LocationId,

                        Quantity =
                            item.Quantity,

                        UnitPrice =
                            item.UnitPrice,

                        TotalPrice =
                            (item.Quantity *
                             item.UnitPrice) -
                            item.Discount,

                        Discount =
                            item.Discount,

                        CreatedAt =
                            DateTime.UtcNow,

                        CreatedBy =
                            userId
                    };

                await _unitOfWork.SaleItems
                    .AddAsync(saleItem);


                // ====================================================
                // 5. CREATE INVENTORY TRANSACTION
                // ====================================================

                var inventoryTransaction =
                    new Domain.Models.InventoryTransaction
                    {
                        MedicineId =
                            item.MedicineId,

                        InventoryBatchId =
                            item.InventoryBatchId,

                        MedicineUnitId =
                            item.MedicineUnitId,

                        // IMPORTANT:
                        // Use LocationId or LocationId according
                        // to your actual InventoryTransaction entity.
                        LocationId =
                            item.LocationId,

                        Quantity =
                            item.Quantity,

                        UnitCost =
                            item.UnitPrice,

                        TotalCost =
                            item.Quantity *
                            item.UnitPrice,

                        TransactionType =
                            InventoryTransactionType.Sale,

                        ReferenceType =
                            InventoryReferenceType.Sale,

                        ReferenceID =
                            sale.Id,

                        TransactionDate =
                            DateTime.UtcNow,

                        ReferenceNumber =
                            dto.InvoiceNumber,

                        Description =
                            $"Sale #{sale.Id}",

                        CreatedAt =
                            DateTime.UtcNow,

                        CreatedBy =
                            userId
                    };

                await _unitOfWork.InventoryTransactions
                    .AddAsync(
                        inventoryTransaction);
            }


            // ========================================================
            // 6. SAVE EVERYTHING
            // ========================================================

            await _unitOfWork.SaveAsync(
                cancellationToken);


            // ========================================================
            // 7. COMMIT
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
}