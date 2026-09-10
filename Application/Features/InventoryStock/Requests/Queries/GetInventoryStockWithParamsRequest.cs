using Application.Dtos.Common;
using Application.Dtos.InventoryStock;
using MediatR;

public class GetInventoryStockWithParamsRequest: IRequest<PaginatedResult<InventoryStockDto>>
{
    public string Search { get; set; }
    public string SortBy { get; set; }
    public string SortDirection { get; set; }
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}