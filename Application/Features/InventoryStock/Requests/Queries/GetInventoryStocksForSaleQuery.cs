using Application.Dtos.Sale;
using MediatR;

namespace Application.Features.InventoryStock.Requests.Queries
{
    public class GetInventoryStocksForSaleQuery : IRequest<List<InventoryStockForSaleDto>>
    {
        public int? MedicineId { get; set; }
    }
}