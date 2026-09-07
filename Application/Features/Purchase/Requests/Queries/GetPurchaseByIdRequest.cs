using Application.Dtos.Purchase;
using MediatR;

namespace Application.Features.Purchase.Requests.Queries
{
    public class GetPurchaseByIdRequest : IRequest<PurchaseDto?>
    {
        public int Id { get; set; }
    }
}