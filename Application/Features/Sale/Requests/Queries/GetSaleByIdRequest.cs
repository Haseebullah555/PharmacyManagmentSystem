using Application.Dtos.Sale;
using MediatR;

namespace Application.Features.Sale.Requests.Queries
{
    public class GetSaleByIdRequest : IRequest<SaleDto>
    {
        public int Id { get; set; }
    }
}