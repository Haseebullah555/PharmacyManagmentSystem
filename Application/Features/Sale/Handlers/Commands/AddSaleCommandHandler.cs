using System.ComponentModel.DataAnnotations;
using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Sale.Requests.Commands;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class AddSaleCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICurrentUserRepository currentUser) : IRequestHandler<AddSaleCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserRepository _currentUser = currentUser;

    public async Task Handle(AddSaleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.AddSaleDto;

        if (dto.Items == null || !dto.Items.Any())
            throw new Exception("Sale must contain at least one item.");

        if (dto.PaidAmount < 0)
            throw new Exception("Paid amount cannot be negative.");

        if (dto.Discount < 0)
            throw new Exception("Discount cannot be negative.");

        // Calculate total before discount
        var itemsTotal = dto.Items.Sum(x =>
        {
            var itemGross = x.Quantity * x.UnitPrice;

            if (x.Discount < 0)
                throw new Exception("Item discount cannot be negative.");

            if (x.Discount > itemGross)
                throw new Exception("Item discount cannot be greater than item total.");

            return itemGross - x.Discount;
        });

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

        var totalAmount = itemsTotal - dto.Discount;

        if (totalAmount < 0)
            throw new Exception("Discount cannot be greater than the total amount.");

        if (dto.PaidAmount > totalAmount)
            throw new Exception("Paid amount cannot be greater than the total amount.");

        await using var transaction =
            await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userId = _currentUser.GetCurrentLoggedInUserId();

            // -----------------------------------------
            // 1. Create Sale
            // -----------------------------------------

            var sale = new Domain.Models.Sale
            {
                SaleDate = dto.SaleDate,
                CustomerID = dto.CustomerID,
                CurrencyID = dto.CurrencyID,

                TotalAmount = totalAmount,
                PaidAmount = dto.PaidAmount,
                UnpaidAmount = totalAmount - dto.PaidAmount,

                Discount = dto.Discount,

                InvoiceNumber = dto.InvoiceNumber,
                Remarks = dto.Remarks,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            await _unitOfWork.Sales.AddAsync(sale);

            // Save to generate Sale ID
            await _unitOfWork.SaveAsync(cancellationToken);


            // -----------------------------------------
            // 2. Process Sale Items
            // -----------------------------------------

            foreach (var item in dto.Items)
            {
                if (item.Quantity <= 0)
                    throw new Exception(
                        "Sale quantity must be greater than zero.");

                if (item.UnitPrice < 0)
                    throw new Exception(
                        "Unit price cannot be negative.");


                // -----------------------------------------
                // Find Inventory Batch
                // -----------------------------------------

                var batch = await _unitOfWork.InventoryBatches
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == item.InventoryBatchID
                             && x.MedicineID == item.MedicineID
                             && x.IsActive,
                        cancellationToken);

                if (batch == null)
                    throw new Exception(
                        $"Inventory batch not found for Medicine ID {item.MedicineID}.");


                // -----------------------------------------
                // Find Inventory Stock
                // -----------------------------------------

                var stock = await _unitOfWork.InventoryStocks
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.InventoryBatchID == item.InventoryBatchID &&
                            x.LocationID == item.LocationID &&
                            x.MedicineUnitID == item.MedicineUnitID,
                        cancellationToken);
                if (stock.Quantity < item.Quantity)
                {
                    throw new ValidationException(
                        $"Insufficient stock for {medicineName}. " +
                        $"Available: {stock.Quantity}, Requested: {item.Quantity}.");
                }
                if (stock == null)
                    throw new Exception(
                        $"No inventory stock found for Medicine ID {item.MedicineID}.");

                // -----------------------------------------
                // Check Available Quantity
                // -----------------------------------------

                if (stock.Quantity < item.Quantity)
                {
                    throw new Exception(
                        $"Insufficient stock. Available: {stock.Quantity}, Requested: {item.Quantity}.");
                }


                // -----------------------------------------
                // Reduce Stock
                // -----------------------------------------

                stock.Quantity -= item.Quantity;

                stock.UpdatedAt = DateTime.UtcNow;
                stock.UpdateBy = userId;

                _unitOfWork.InventoryStocks.Update(stock);


                // -----------------------------------------
                // Create Sale Item
                // -----------------------------------------

                var saleItem = new Domain.Models.SaleItem
                {
                    SaleID = sale.Id,

                    MedicineID = item.MedicineID,
                    MedicineUnitID = item.MedicineUnitID,

                    InventoryBatchID = item.InventoryBatchID,
                    LocationID = item.LocationID,

                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,

                    TotalPrice =
                        (item.Quantity * item.UnitPrice) -
                        item.Discount,

                    Discount = item.Discount,

                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                };

                await _unitOfWork.SaleItems.AddAsync(saleItem);


                // -----------------------------------------
                // Create Inventory Transaction
                // -----------------------------------------

                var inventoryTransaction =
                    new Domain.Models.InventoryTransaction
                    {
                        MedicineID = item.MedicineID,

                        InventoryBatchID =
                            item.InventoryBatchID,

                        MedicineUnitID =
                            item.MedicineUnitID,

                        LocationID =
                            item.LocationID,

                        Quantity = item.Quantity,

                        TransactionType =
                            InventoryTransactionType.Sale,

                        TransactionDate =
                            DateTime.UtcNow,

                        ReferenceNumber =
                            dto.InvoiceNumber,

                        Description =
                            $"Sale #{sale.Id}",

                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    };

                await _unitOfWork.InventoryTransactions
                    .AddAsync(inventoryTransaction);
            }


            // -----------------------------------------
            // 3. Save Everything
            // -----------------------------------------

            await _unitOfWork.SaveAsync(cancellationToken);


            // -----------------------------------------
            // 4. Commit Transaction
            // -----------------------------------------

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}