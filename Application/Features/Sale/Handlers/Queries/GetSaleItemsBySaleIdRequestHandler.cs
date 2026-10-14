using Application.Contracts.Interfaces.Common;
using Application.Dtos.SaleItem;
using Application.Features.Sale.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Sale.Handlers.Queries
{
    public class GetSaleItemsBySaleIdRequestHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetSaleItemsBySaleIdRequest, List<SaleItemDto>>
    {
        public async Task<List<SaleItemDto>> Handle(
            GetSaleItemsBySaleIdRequest request,
            CancellationToken cancellationToken)
        {
            return await unitOfWork.SaleItems
                .Query()
                .AsNoTracking()
                .Where(item => item.SaleID == request.SaleId)
                .Select(item => new SaleItemDto
                {
                    Id = item.Id,
                    MedicineId = item.MedicineId,
                    MedicineName = item.Medicine.TradeName,
                    MedicineUnitId = item.MedicineUnitId,
                    MedicineUnitName = item.MedicineUnit.Unit.Name,
                    InventoryBatchId = item.InventoryBatchId,
                    BatchNumber = item.InventoryBatch.BatchNumber,
                    ExpiryDate = item.InventoryBatch.ExpiryDate,
                    LocationId = item.LocationId,
                    LocationName = item.Location.LocationName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    TotalPrice = item.TotalPrice
                })
                .ToListAsync(cancellationToken);
        }
    }
}
