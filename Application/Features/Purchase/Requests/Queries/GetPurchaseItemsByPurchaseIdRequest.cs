using Application.Dtos.PurchaseItem;
using MediatR;

namespace Application.Features.Purchase.Requests.Queries
{
    public class GetPurchaseItemsByPurchaseIdRequest : IRequest<List<PurchaseItemDto>>
    {
        public int PurchaseId { get; set; }
    }
}