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
        var stockQuery = _unitOfWork.InventoryStocks
            .Query()
            .AsNoTracking()
            .Where(x => x.Quantity > 0);

        // Search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            stockQuery = stockQuery.Where(x =>
                x.InventoryBatch.Medicine.TradeName.Contains(search) ||
                x.InventoryBatch.Medicine.GenericName.Contains(search)
            );
        }

        // A medicine may be stored in multiple batches, units, and locations.
        // This page is a stock summary, so it returns one row per medicine.
        var query = stockQuery
            .GroupBy(x => new
            {
                x.InventoryBatch.MedicineId,
                x.InventoryBatch.Medicine.TradeName
            })
            .Select(group => new InventoryStockDto
            {
                Id = group.Key.MedicineId,
                MedicineId = group.Key.MedicineId,
                MedicineName = group.Key.TradeName,
                Quantity = group.Sum(x => x.Quantity)
            });

        var totalRecords = await query.CountAsync(cancellationToken);

        // Sorting
        query = request.SortBy?.ToLower() switch
        {
            "medicine" => request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.MedicineName)
                : query.OrderBy(x => x.MedicineName),

            "quantity" => request.SortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(x => x.Quantity)
                : query.OrderBy(x => x.Quantity),

            _ => query.OrderBy(x => x.MedicineName)
        };

        // Pagination
        var data = await query
            .Skip((request.Page - 1) * request.PerPage)
            .Take(request.PerPage)
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
