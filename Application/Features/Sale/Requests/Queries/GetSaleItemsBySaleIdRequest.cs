using Application.Dtos.SaleItem;
using MediatR;

namespace Application.Features.Sale.Requests.Queries
{
    public class GetSaleItemsBySaleIdRequest : IRequest<List<SaleItemDto>>
    {
        public int SaleId { get; set; }
    }
}
