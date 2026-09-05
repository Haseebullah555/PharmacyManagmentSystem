using Application.Contracts.Interfaces.Common;
using Application.Dtos.Sale;
using Application.Features.Sale.Requests.Queries;
using AutoMapper;
using MediatR;

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

        public async Task<List<InventoryStockForSaleDto>> Handle(
            GetInventoryStocksForSaleQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork
                .Repository<InventoryStock>()
                .Query()
                .Where(x =>
                    x.Quantity > 0 &&
                    x.InventoryBatch.IsActive &&
                    x.MedicineUnit.IsActive &&
                    x.Location.IsActive);

            if (request.MedicineID.HasValue)
            {
                query = query.Where(x =>
                    x.InventoryBatch.MedicineID == request.MedicineID.Value);
            }

            return await query
                .Select(x => new InventoryStockForSaleDto
                {
                    Id = x.Id,

                    MedicineID = x.InventoryBatch.MedicineID,
                    MedicineName = x.InventoryBatch.Medicine.TradeName,

                    InventoryBatchID = x.InventoryBatchID,
                    BatchNumber = x.InventoryBatch.BatchNumber,

                    MedicineUnitID = x.MedicineUnitID,
                    UnitName = x.MedicineUnit.Unit.Name,
                    UnitShortName = x.MedicineUnit.Unit.ShortName,

                    LocationID = x.LocationID,
                    LocationName = x.Location.Name,

                    Quantity = x.Quantity
                })
                .ToListAsync(cancellationToken);
        }
    }
}