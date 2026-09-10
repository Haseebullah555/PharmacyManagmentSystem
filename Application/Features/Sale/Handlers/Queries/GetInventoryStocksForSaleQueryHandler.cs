using Application.Contracts.Interfaces.Common;
using Application.Dtos.Sale;
using Application.Features.Sale.Requests.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Sale.Handlers.Queries
{
    public class GetInventoryStocksForSaleQueryHandler
    : IRequestHandler<
        GetInventoryStocksForSaleQuery,
        List<InventoryStockForSaleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInventoryStocksForSaleQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<InventoryStockForSaleDto>> Handle(GetInventoryStocksForSaleQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork
                .InventoryStocks
                .Query()
                .Where(x =>
                    x.Quantity > 0 &&
                    x.InventoryBatch.IsActive &&
                    // x.MedicineUnit.IsActive &&
                    x.Location.IsActive);

            if (request.MedicineId.HasValue)
            {
                query = query.Where(x =>
                    x.InventoryBatch.MedicineId == request.MedicineId.Value);
            }

            return await query
                .Select(x => new InventoryStockForSaleDto
                {
                    Id = x.Id,

                    MedicineId = x.InventoryBatch.MedicineId,
                    MedicineName = x.InventoryBatch.Medicine.TradeName,

                    InventoryBatchId = x.InventoryBatchId,
                    BatchNumber = x.InventoryBatch.BatchNumber,

                    MedicineUnitId = x.MedicineUnitId,
                    UnitName = x.MedicineUnit.Unit.Name,
                    UnitShortName = x.MedicineUnit.Unit.ShortName,

                    LocationId = x.LocationId,
                    LocationName = x.Location.LocationName,

                    Quantity = x.Quantity
                }).ToListAsync(cancellationToken);
        }
    }
}