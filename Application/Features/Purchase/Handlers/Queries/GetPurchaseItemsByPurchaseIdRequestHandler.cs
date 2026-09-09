using Application.Contracts.Interfaces.Common;
using Application.Dtos.PurchaseItem;
using Application.Features.Purchase.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Purchase.Handlers.Queries
{
    public class GetPurchaseItemsByPurchaseIdRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetPurchaseItemsByPurchaseIdRequest, List<PurchaseItemDto>>
    {
        public async Task<List<PurchaseItemDto>> Handle(GetPurchaseItemsByPurchaseIdRequest request, CancellationToken cancellationToken)
        {
            var purchaseItems = _unitOfWork.PurchaseItems.Query().AsNoTracking().Where(x => x.PurchaseId == request.PurchaseId).Select(x => new PurchaseItemDto
            {
                Id = x.Id,

                MedicineId = x.MedicineId,
                MedicineName = x.Medicine.TradeName,

                MedicineUnitId = x.MedicineUnitId,
                UnitName = x.MedicineUnit.Unit.Name,
                UnitShortName = x.MedicineUnit.Unit.ShortName,

                InventoryBatchId = x.InventoryBatchId,
                BatchNumber = x.InventoryBatch.BatchNumber,
                ManufacturingDate = x.InventoryBatch.ManufacturingDate,
                ExpiryDate = x.InventoryBatch.ExpiryDate,

                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalPrice = x.TotalPrice
            }).ToList();
            return purchaseItems;
        }
    }
}