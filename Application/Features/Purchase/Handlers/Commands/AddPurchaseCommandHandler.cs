using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Purchase.Requests.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Purchase.Handlers.Commands
{
    public class AddPurchaseCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserRepository currentUser)
    : IRequestHandler<AddPurchaseCommand>
    {
        public async Task Handle(
            AddPurchaseCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.AddPurchaseDto;
            var userId = currentUser.GetCurrentLoggedInUserId();

            if (dto.Items == null || !dto.Items.Any())
                throw new Exception(
                    "Purchase must contain at least one item.");

            if (dto.PaidAmount < 0)
                throw new Exception(
                    "Paid amount cannot be negative.");

            var totalAmount = dto.Items.Sum(x =>
                x.Quantity * x.UnitPrice);

            if (dto.PaidAmount > totalAmount)
                throw new Exception(
                    "Paid amount cannot be greater than total amount.");

            await using var transaction =
                await unitOfWork.BeginTransactionAsync();

            try
            {
                // ============================================
                // 1. CREATE PURCHASE
                // ============================================

                var purchase = new Domain.Models.Purchase
                {
                    PurchaseDate = dto.PurchaseDate,
                    InvoiceNumber = dto.InvoiceNumber,
                    SupplierID = dto.SupplierID,
                    CurrencyID = dto.CurrencyID,

                    TotalAmount = totalAmount,
                    PaidAmount = dto.PaidAmount,
                    UnpaidAmount =
                        totalAmount - dto.PaidAmount,

                    Remarks = dto.Remarks,

                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                await unitOfWork.Purchases.AddAsync(purchase);

                await unitOfWork.SaveAsync(
                    cancellationToken);


                // ============================================
                // 2. PROCESS PURCHASE ITEMS
                // ============================================

                foreach (var itemDto in dto.Items)
                {
                    // ========================================
                    // Find existing batch
                    // ========================================

                    var batch =
                        await unitOfWork.InventoryBatches
                            .Query()
                            .FirstOrDefaultAsync(
                                x =>
                                    x.MedicineID ==
                                        itemDto.MedicineID &&
                                    x.BatchNumber ==
                                        itemDto.BatchNumber,
                                cancellationToken);


                    // ========================================
                    // Create batch if it doesn't exist
                    // ========================================

                    if (batch == null)
                    {
                        batch = new InventoryBatch
                        {
                            MedicineID =
                                itemDto.MedicineID,

                            BatchNumber =
                                itemDto.BatchNumber,

                            ManufacturingDate =
                                itemDto.ManufacturingDate,

                            ExpiryDate =
                                itemDto.ExpiryDate,

                            IsActive = true,

                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = userId
                        };

                        await unitOfWork.InventoryBatches
                            .AddAsync(batch);

                        await unitOfWork.SaveAsync(
                            cancellationToken);
                    }


                    // ========================================
                    // 3. CREATE PURCHASE ITEM
                    // ========================================

                    var purchaseItem = new PurchaseItem
                    {
                        PurchaseID = purchase.Id,

                        MedicineID =
                            itemDto.MedicineID,

                        MedicineUnitID =
                            itemDto.MedicineUnitID,

                        Quantity =
                            itemDto.Quantity,

                        UnitPrice =
                            itemDto.UnitPrice,

                        TotalPrice =
                            itemDto.Quantity *
                            itemDto.UnitPrice,

                        InventoryBatchID =
                            batch.Id,

                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    };

                    await unitOfWork.PurchaseItems
                        .AddAsync(purchaseItem);


                    // ========================================
                    // 4. INVENTORY STOCK
                    // ========================================

                    var stock =
                        await unitOfWork.InventoryStocks
                            .Query()
                            .FirstOrDefaultAsync(
                                x =>
                                    x.InventoryBatchID ==
                                        batch.Id &&
                                    x.LocationID ==
                                        itemDto.LocationID &&
                                    x.MedicineUnitID ==
                                        itemDto.MedicineUnitID,
                                cancellationToken);


                    if (stock == null)
                    {
                        stock = new InventoryStock
                        {
                            InventoryBatchID =
                                batch.Id,

                            LocationID =
                                itemDto.LocationID,

                            MedicineUnitID =
                                itemDto.MedicineUnitID,

                            Quantity =
                                itemDto.Quantity,

                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = userId
                        };

                        await unitOfWork.InventoryStocks
                            .AddAsync(stock);
                    }
                    else
                    {
                        stock.Quantity +=
                            itemDto.Quantity;

                        stock.UpdatedAt =
                            DateTime.UtcNow;

                        stock.UpdateBy =
                            userId;

                        unitOfWork.InventoryStocks
                            .Update(stock);
                    }


                    // ========================================
                    // 5. INVENTORY TRANSACTION
                    // ========================================

                    var inventoryTransaction =
                        new InventoryTransaction
                        {
                            MedicineID =
                                itemDto.MedicineID,

                            InventoryBatchID =
                                batch.Id,

                            MedicineUnitID =
                                itemDto.MedicineUnitID,

                            LocationID =
                                itemDto.LocationID,

                            Quantity =
                                itemDto.Quantity,

                            TransactionType =
                                InventoryTransactionType.Purchase,

                            TransactionDate =
                                DateTime.UtcNow,

                            ReferenceNumber =
                                dto.InvoiceNumber,

                            Description =
                                $"Purchase #{purchase.Id}",

                            CreatedAt =
                                DateTime.UtcNow,

                            CreatedBy =
                                userId
                        };

                    await unitOfWork.InventoryTransactions
                        .AddAsync(
                            inventoryTransaction);
                }


                // ============================================
                // 6. SAVE
                // ============================================

                await unitOfWork.SaveAsync(
                    cancellationToken);


                // ============================================
                // 7. COMMIT
                // ============================================

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
}
