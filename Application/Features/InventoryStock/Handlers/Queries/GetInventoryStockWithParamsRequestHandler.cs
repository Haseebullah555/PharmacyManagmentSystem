using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.InventoryStock;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetInventoryStockWithParamsRequestHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetInventoryStockWithParamsRequest, PaginatedResult<InventoryStockDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PaginatedResult<InventoryStockDto>> Handle(
        GetInventoryStockWithParamsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.InventoryStocks
            .Query()
            .AsNoTracking()
            .Where(x => x.Quantity > 0);

        // Search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.InventoryBatch.Medicine.TradeName.Contains(search) ||
                x.InventoryBatch.Medicine.GenericName.Contains(search) ||
                x.InventoryBatch.BatchNumber.Contains(search) ||
                x.Location.LocationName.Contains(search) ||
                x.MedicineUnit.Unit.Name.Contains(search) ||
                x.MedicineUnit.Unit.ShortName.Contains(search)
            );
        }

        // Total records
        var totalRecords = await query.CountAsync(cancellationToken);

        // Sorting
        query = request.SortBy?.ToLower() switch
        {
            "medicine" => request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.InventoryBatch.Medicine.TradeName)
                : query.OrderBy(x => x.InventoryBatch.Medicine.TradeName),

            "batch" => request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.InventoryBatch.BatchNumber)
                : query.OrderBy(x => x.InventoryBatch.BatchNumber),

            "expirydate" => request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.InventoryBatch.ExpiryDate)
                : query.OrderBy(x => x.InventoryBatch.ExpiryDate),

            "location" => request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.Location.LocationName)
                : query.OrderBy(x => x.Location.LocationName),

            "quantity" => request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.Quantity)
                : query.OrderBy(x => x.Quantity),

            _ => query.OrderBy(x => x.InventoryBatch.Medicine.TradeName)
        };

        // Pagination
        var data = await query
            .Skip((request.Page - 1) * request.PerPage)
            .Take(request.PerPage)
            .Select(x => new InventoryStockDto
            {
                Id = x.Id,

                MedicineId = x.InventoryBatch.MedicineId,
                MedicineName = x.InventoryBatch.Medicine.TradeName,

                InventoryBatchId = x.InventoryBatchId,
                BatchNumber = x.InventoryBatch.BatchNumber,

                ManufacturingDate = x.InventoryBatch.ManufacturingDate,
                ExpiryDate = x.InventoryBatch.ExpiryDate,

                MedicineUnitId = x.MedicineUnitId,
                UnitName = x.MedicineUnit.Unit.Name,
                UnitShortName = x.MedicineUnit.Unit.ShortName,

                LocationId = x.LocationId,
                LocationName = x.Location.LocationName,

                Quantity = x.Quantity
            })
            .ToListAsync(cancellationToken);
        return new PaginatedResult<InventoryStockDto>
        {
            Data = data,
            Total = totalRecords,
            CurrentPage = request.Page,
            PerPage = request.PerPage
        };
    }
}