using Application.Dtos.Sale;
using MediatR;

namespace Application.Features.Sale.Requests.Queries
{
    public class GetInventoryStocksForSaleQuery
      : IRequest<List<InventoryStockForSaleDto>>
    {
        public int? MedicineID { get; set; }
    }
}