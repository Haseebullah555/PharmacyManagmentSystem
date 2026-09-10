using Application.Contracts.Interfaces.Common;
using Application.Dtos.Purchase;
using Application.Dtos.PurchaseItem;
using Application.Features.Purchase.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Purchase.Handlers.Queries
{
    public class GetPurchaseByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetPurchaseByIdRequest, PurchaseDto?>
    {

        public async Task<PurchaseDto?> Handle(GetPurchaseByIdRequest request, CancellationToken cancellationToken)
        {
            var purchase = await _unitOfWork.Purchases
            .Query()
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new PurchaseDto
            {
                Id = x.Id,
                PurchaseDate = x.PurchaseDate,
                InvoiceNumber = x.InvoiceNumber,
                SupplierId = x.SupplierId,
                Supplier = x.Supplier.SupplierName,
                CurrencyId = x.CurrencyId,
                Currency = x.Currency.CurrencyName,
                ExchangeRate = x.ExchangeRate,
                TotalAmount = x.TotalAmount,
                PaidAmount = x.PaidAmount,
                UnpaidAmount = x.UnpaidAmount,
                Remarks = x.Remarks,

                Items = x.Items.Select(item => new PurchaseItemDto
                {
                    Id = item.Id,

                    MedicineId = item.MedicineId,
                    MedicineName = item.Medicine.TradeName,

                    MedicineUnitId = item.MedicineUnitId,
                    MedicineUnitName = item.MedicineUnit.Unit.Name,

                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,

                    InventoryBatchId = item.InventoryBatchId,

                    BatchNumber = item.InventoryBatch.BatchNumber,
                    ManufacturingDate = item.InventoryBatch.ManufacturingDate,
                    ExpiryDate = item.InventoryBatch.ExpiryDate,

                    LocationId = item.InventoryBatch.Stocks
                        .Select(stock => stock.LocationId)
                        .FirstOrDefault(),
                    LocationName = item.InventoryBatch.Stocks
                        .Select(stock => stock.Location.LocationName)
                        .FirstOrDefault()
                }).ToList()
            }).FirstOrDefaultAsync();

            if (purchase == null)
            {
                return null;
            }
            return purchase;
        }
    }
}